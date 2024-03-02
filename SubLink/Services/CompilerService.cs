using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace xyz.yewnyx.SubLink.Services;

internal partial class CompilerService {
    private AssemblyLoadContext? _assemblyLoadContext;

    private static readonly string[] _baseUsings = {
        "VRC.OSCQuery",
        "BuildSoft.VRChat.Osc",
        "BuildSoft.VRChat.Osc.Avatar",
        "BuildSoft.VRChat.Osc.Chatbox",
        "BuildSoft.VRChat.Osc.Delegate",
        "BuildSoft.VRChat.Osc.Input",
        "BuildSoft.VRChat.Osc.Tracking",
        "BuildSoft.VRChat.Osc.Utility",
        "XSNotifications",
        "XSNotifications.Enum",
        "XSNotifications.Exception",
        "XSNotifications.Helpers",
        "BuildSoft.OscCore.UnityObjects",
    };

    private static readonly string[] _baseAssemblies = {
        "vrcosclib",
        "vrc-oscquery-lib",
        "BuildSoft.OscCore",
        "SubLink",
        "SubLink.References",
    };

    private readonly ILogger _logger;
    private readonly ScriptGlobals _globals;
    private readonly Type GlobalsType;

    public CompilerService(ILogger logger, ScriptGlobals globals) {
        _logger = logger;
        _globals = globals;
        _globals.logger = _logger;
        GlobalsType = typeof(ScriptGlobals);
    }

    public async Task<Func<Task<object?>>> CompileSource(IFileInfo fileInfo, IServiceProvider serviceProvider,
        CancellationToken stoppingToken) {
        _globals.serviceProvider = serviceProvider;

        if (_assemblyLoadContext != null) {
            _assemblyLoadContext.Unload();
            _assemblyLoadContext = null;
        }

        _assemblyLoadContext = new AssemblyLoadContext("SubLinkScript", true);

        List<string> serviceSymbols = new();

        // Combine base usings and assemblies with service-specific ones
        List<string> usings = new();
        usings.AddRange(_baseUsings);

        List<string> assemblies = new();
        assemblies.AddRange(_baseAssemblies);

        var assemblyReferences = Net70.References.All.ToList();

        foreach (var platform in HostGlobals.Platforms.Values) {
            platform.Entry.SetLogger(_logger);
            platform.Entry.SetServiceProvider(serviceProvider);

            serviceSymbols.Add(platform.Entry.GetServiceSymbol());

            usings.AddRange(platform.Entry.GetAdditionalUsings());
            assemblies.AddRange(platform.Entry.GetAdditionalAssemblies());

            platform.Entry.AppendRules(_globals.rules);

            unsafe {
                platform.Assembly.TryGetRawMetadata(out var blob, out var length);
                var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                var reference = assemblyMetadata.GetReference();
                assemblyReferences.Add(reference);
            }
        }

        // Parse source file
        SyntaxTree syntaxTree;
        await using (var scriptStream = fileInfo.CreateReadStream()) {
            SourceText srcText = SourceText.From(scriptStream, Encoding.UTF8);
            PatchLegacyScript(ref srcText);

            syntaxTree = CSharpSyntaxTree.ParseText(
                srcText,
                CSharpParseOptions.Default
                    .WithLanguageVersion(LanguageVersion.Latest)
                    .WithKind(SourceCodeKind.Script)
                    .WithPreprocessorSymbols(serviceSymbols),
                fileInfo.PhysicalPath!,
                stoppingToken);
        }

        // Set up the compilation
        var csco = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(true)
            .WithOptimizationLevel(OptimizationLevel.Debug)
            .WithDeterministic(true)
            .WithUsings(usings);

        var additionalAssemblies = new HashSet<Assembly>();

        foreach (var item in assemblies) {
            additionalAssemblies.Add(Assembly.Load(item));
        }

        foreach (var assembly in additionalAssemblies) {
            unsafe {
                assembly.TryGetRawMetadata(out var blob, out var length);
                var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                var reference = assemblyMetadata.GetReference();
                assemblyReferences.Add(reference);
            }
        }

        var assemblyFilename = Path.GetExtension(fileInfo.PhysicalPath)!;
        var compilation = CSharpCompilation.CreateScriptCompilation(assemblyFilename, syntaxTree, assemblyReferences,
            csco, globalsType: GlobalsType);

        // Compile the file and show errors if any
        await using var ms = new MemoryStream();
        EmitResult result;

        try {
            result = compilation.Emit(ms, cancellationToken: stoppingToken);
        } catch (Exception ex) {
            _logger.Error(ex, "Failed to emit DLL");
            throw;
        }

        if (!result.Success) {
            var diagnostics = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

            _logger.Error("Failed to compile custom code");

            foreach (var diagnostic in diagnostics) {
                _logger.Error("Diagnostic {id}, {message}", diagnostic.Id, diagnostic.GetMessage());
            }

            throw new InvalidProgramException();
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assm = _assemblyLoadContext.LoadFromStream(ms);

        var entryPoint = compilation.GetEntryPoint(CancellationToken.None)!;
        var type = assm.GetType($"{entryPoint.ContainingNamespace.MetadataName}.{entryPoint.ContainingType.MetadataName}")!;
        var entryPointMethod = type.GetMethod(entryPoint.MetadataName)!;

        var submission = (Func<object[], Task>)entryPointMethod.CreateDelegate(typeof(Func<object[], Task>));
        return () => (Task<object?>)submission(new object[] { _globals, null! });
    }

    private static void PatchLegacyScript(ref SourceText srcText) {
        bool patched = false;
        var srcStr = srcText.ToString();

        if (TwitchLegacyRegex().IsMatch(srcStr) && !TwitchModernRegex().IsMatch(srcStr)) {
            patched = true;
            var idx = TwitchLegacyRegex().Match(srcStr).Index;

            var sb = new StringBuilder(srcStr[..idx]);
            sb.AppendLine("var twitch = (TwitchRules)rules[\"Twitch\"];");
            sb.Append(srcStr.AsSpan(idx));

            srcStr = sb.ToString();
        }

        if (KickLegacyRegex().IsMatch(srcStr) && !KickModernRegex().IsMatch(srcStr)) {
            patched = true;
            var idx = KickLegacyRegex().Match(srcStr).Index;

            var sb = new StringBuilder(srcStr[..idx]);
            sb.AppendLine("var kick = (KickRules)rules[\"Kick\"];");
            sb.Append(srcStr.AsSpan(idx));

            srcStr = sb.ToString();
        }

        if (StreamPadLegacyRegex().IsMatch(srcStr) && !StreamPadModernRegex().IsMatch(srcStr)) {
            patched = true;
            var idx = StreamPadLegacyRegex().Match(srcStr).Index;

            var sb = new StringBuilder(srcStr[..idx]);
            sb.AppendLine("var streamPad = (StreamPadRules)rules[\"StreamPad\"];");
            sb.Append(srcStr.AsSpan(idx));

            srcStr = sb.ToString();
        }

        if (StreamElementsLegacyRegex().IsMatch(srcStr) && !StreamElementsModernRegex().IsMatch(srcStr)) {
            patched = true;
            var idx = StreamElementsLegacyRegex().Match(srcStr).Index;

            var sb = new StringBuilder(srcStr[..idx]);
            sb.AppendLine("var streamElements = (StreamElementsRules)rules[\"StreamElements\"];");
            sb.Append(srcStr.AsSpan(idx));

            srcStr = sb.ToString();
        }

        if (FanslyLegacyRegex().IsMatch(srcStr) && !FanslyModernRegex().IsMatch(srcStr)) {
            patched = true;
            var idx = FanslyLegacyRegex().Match(srcStr).Index;

            var sb = new StringBuilder(srcStr[..idx]);
            sb.AppendLine("var fansly = (FanslyRules)rules[\"Fansly\"];");
            sb.Append(srcStr.AsSpan(idx));

            srcStr = sb.ToString();
        }

        if (patched)
            srcText = SourceText.From(srcStr, Encoding.UTF8);
    }

    [GeneratedRegex(@".*twitch\.\w*\(")]
    private static partial Regex TwitchLegacyRegex();
    [GeneratedRegex(@"\bvar\s+twitch\s+=\s+\(TwitchRules\)rules\[""Twitch""\];")]
    private static partial Regex TwitchModernRegex();

    [GeneratedRegex(@".*kick\.\w*\(")]
    private static partial Regex KickLegacyRegex();
    [GeneratedRegex(@"\bvar\s+kick\s+=\s+\(KickRules\)rules\[""Kick""\];")]
    private static partial Regex KickModernRegex();

    [GeneratedRegex(@".*streamPad\.\w*\(")]
    private static partial Regex StreamPadLegacyRegex();
    [GeneratedRegex(@"\bvar\s+streamPad\s+=\s+\(StreamPadRules\)rules\[""StreamPad""\];")]
    private static partial Regex StreamPadModernRegex();

    [GeneratedRegex(@".*streamElements\.\w*\(")]
    private static partial Regex StreamElementsLegacyRegex();
    [GeneratedRegex(@"\bvar\s+streamElements\s+=\s+\(StreamElementsRules\)rules\[""StreamElements""\];")]
    private static partial Regex StreamElementsModernRegex();

    [GeneratedRegex(@".*fansly\.\w*\(")]
    private static partial Regex FanslyLegacyRegex();
    [GeneratedRegex(@"\bvar\s+fansly\s+=\s+\(FanslyRules\)rules\[""Fansly""\];")]
    private static partial Regex FanslyModernRegex();
}
