using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using System.Text;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace xyz.yewnyx.SubLink;

public abstract class BaseCompilerService<TGlobals> where TGlobals : IGlobals {
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
        "XSNotifications",
        "SubLinkCommon",
    };

    protected readonly ILogger _logger;
    protected readonly TGlobals _globals;

    protected Type GlobalsType => typeof(TGlobals); 

    protected abstract string ServiceSymbol { get; }

    protected virtual string[] ServiceUsings { get => Array.Empty<string>(); }

    protected virtual string[] ServiceAssemblies { get => Array.Empty<string>(); }

    public BaseCompilerService(ILogger logger, TGlobals globals) {
        _logger = logger;
        globals.logger = logger;
        _globals = globals;
    }

    public async Task<Func<Task<object?>>> CompileSource(IFileInfo fileInfo, CancellationToken stoppingToken)
    {
        if (_assemblyLoadContext != null) {
            _assemblyLoadContext.Unload();
            _assemblyLoadContext = null;
        }

        _assemblyLoadContext = new AssemblyLoadContext("SubLinkScript", true);

        // Combine base usings and assemblies with service-specific ones
        string[] usings = new string[_baseUsings.Length + ServiceUsings.Length];
        _baseUsings.CopyTo(usings, 0);
        ServiceUsings.CopyTo(usings, _baseUsings.Length);

        string[] assemblies = new string[_baseAssemblies.Length + ServiceAssemblies.Length];
        _baseAssemblies.CopyTo(assemblies, 0);
        ServiceAssemblies.CopyTo(assemblies, _baseAssemblies.Length);

        // Parse source file
        SyntaxTree syntaxTree;
        await using (var scriptStream = fileInfo.CreateReadStream()) {
            syntaxTree = CSharpSyntaxTree.ParseText(
                SourceText.From(scriptStream, Encoding.UTF8),
                CSharpParseOptions.Default
                    .WithLanguageVersion(LanguageVersion.Latest)
                    .WithKind(SourceCodeKind.Script)
                    .WithPreprocessorSymbols(ServiceSymbol),
                fileInfo.PhysicalPath!,
                stoppingToken);
        }

        // Set up the compilation
        var csco = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(true)
            .WithOptimizationLevel(OptimizationLevel.Debug)
            .WithDeterministic(true)
            .WithUsings(usings);

        var assemblyReferences = Net70.References.All.ToList();

        var additionalAssemblies = new HashSet<Assembly> {
            typeof(ILogger).Assembly,
            GlobalsType.Assembly
        };

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
        var compilation
            = CSharpCompilation.CreateScriptCompilation(assemblyFilename, syntaxTree, assemblyReferences, csco,
                globalsType: GlobalsType);

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
}
