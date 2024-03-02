using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public class CompilerService : BaseCompilerService<StreamElementsGlobals> {
    [UsedImplicitly]
    private readonly IStreamElementsRules _streamElements;

    protected override string ServiceSymbol { get => "SUBLINK_STREAMELEMENTS"; }

    protected override string[] ServiceUsings {
        get => new string[] {
            "xyz.yewnyx.SubLink.StreamElements"
        };
    }

    protected override string[] ServiceAssemblies {
        get => Array.Empty<string>();
    }

    public CompilerService(ILogger logger, StreamElementsGlobals globals, IStreamElementsRules streamElements) : base(logger, globals) {
        _streamElements = streamElements;
        globals.streamElements = _streamElements;
        globals.logger = logger;
    }
}