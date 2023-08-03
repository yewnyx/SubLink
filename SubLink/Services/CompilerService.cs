using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.FileProviders;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx {

    public sealed class CompilerService {
        private readonly ILogger _logger;
        
        [UsedImplicitly]
        private readonly IRules _rules;

        private AssemblyLoadContext? _assemblyLoadContext;

        public CompilerService(ILogger logger, IRules rules) {
            _logger = logger;
            _rules = rules;
            
            Globals.rules = _rules;
        }

        public async Task<Func<Task<object?>>> CompileSource(IFileInfo fileInfo, CancellationToken stoppingToken) {
            if (_assemblyLoadContext != null) {
                _assemblyLoadContext.Unload();
                _assemblyLoadContext = null;
            }

            _assemblyLoadContext = new AssemblyLoadContext("SubLinkScript", true);
            
            // Parse source file
            SyntaxTree syntaxTree;
            await using (var scriptStream = fileInfo.CreateReadStream()) {
                syntaxTree = CSharpSyntaxTree.ParseText(
                    SourceText.From(scriptStream, Encoding.UTF8),
                    CSharpParseOptions.Default
                        .WithLanguageVersion(LanguageVersion.Latest)
                        .WithKind(SourceCodeKind.Script),
                    fileInfo.PhysicalPath!,
                    stoppingToken);
            }

            // Set up the compilation
            var csco = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithDeterministic(true)
                .WithUsings(
                    "TwitchLib.EventSub.Core.Models.ChannelGoals",
                    "TwitchLib.EventSub.Core.Models.ChannelPoints",
                    "TwitchLib.EventSub.Core.Models.Charity",
                    "TwitchLib.EventSub.Core.Models.Extensions",
                    "TwitchLib.EventSub.Core.Models.HypeTrain",
                    "TwitchLib.EventSub.Core.Models.Polls",
                    "TwitchLib.EventSub.Core.Models.Predictions",
                    "TwitchLib.EventSub.Core.Models.Subscriptions",
                    "TwitchLib.EventSub.Core.SubscriptionTypes.Channel",
                    "TwitchLib.EventSub.Core.SubscriptionTypes.Drop",
                    "TwitchLib.EventSub.Core.SubscriptionTypes.Extension",
                    "TwitchLib.EventSub.Core.SubscriptionTypes.Stream",
                    "TwitchLib.EventSub.Core.SubscriptionTypes.User",
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
                    "xyz.yewnyx.Globals"
                );

            var assemblyReferences = Net70.References.All.ToList();

            var additionalAssemblies = new HashSet<Assembly> {
                typeof(ILogger).Assembly,
                typeof(Globals).Assembly,
                Assembly.Load("vrcosclib"),
                Assembly.Load("BuildSoft.OscCore"),
                Assembly.Load("TwitchLib.Api"),
                Assembly.Load("TwitchLib.Api.Core"),
                Assembly.Load("TwitchLib.Api.Core.Enums"),
                Assembly.Load("TwitchLib.Api.Core.Interfaces"),
                Assembly.Load("TwitchLib.Api.Core.Models"),
                Assembly.Load("TwitchLib.Api.Helix"),
                Assembly.Load("TwitchLib.Api.Helix.Models"),
                Assembly.Load("TwitchLib.Client"),
                Assembly.Load("TwitchLib.Client.Enums"),
                Assembly.Load("TwitchLib.Client.Models"),
                Assembly.Load("TwitchLib.Communication"),
                Assembly.Load("TwitchLib.EventSub.Core"),
                Assembly.Load("TwitchLib.EventSub.Websockets"),
                Assembly.Load("TwitchLib.PubSub"),
                Assembly.Load("XSNotifications")
            };

            foreach (var assembly in additionalAssemblies) {
                unsafe {
                    assembly.TryGetRawMetadata(out var blob, out var length);
                    var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr) blob, length);
                    var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                    var reference = assemblyMetadata.GetReference();
                    assemblyReferences.Add(reference);
                }
            }

            var assemblyFilename = Path.GetExtension(fileInfo.PhysicalPath)!;
            var compilation
                = CSharpCompilation.CreateScriptCompilation(assemblyFilename, syntaxTree, assemblyReferences, csco,
                    globalsType: typeof(Globals));

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

            Globals.logger = _logger;
            
            var submission = (Func<object[], Task>)entryPointMethod.CreateDelegate(typeof(Func<object[], Task>));
            return () => (Task<object?>)submission(new object[]{null!, null!});
        }
    }
}