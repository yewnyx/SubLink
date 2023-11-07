using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public class CompilerService : BaseCompilerService<StreampadGlobals> {
    [UsedImplicitly]
    private readonly IStreamPadRules _streamPad;

    protected override string ServiceSymbol { get => "SUBLINK_STREAMPAD"; }

    protected override string[] ServiceUsings {
        get => Array.Empty<string>();
    }

    protected override string[] ServiceAssemblies {
        get => Array.Empty<string>();
    }

    public CompilerService(ILogger logger, StreampadGlobals globals, IStreamPadRules streamPad) : base(logger, globals) {
        _streamPad = streamPad;
        globals.streamPad = _streamPad;
        globals.logger = logger;
    }
}