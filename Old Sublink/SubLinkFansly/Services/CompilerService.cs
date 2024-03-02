using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public class CompilerService : BaseCompilerService<FanslyGlobals> {
    [UsedImplicitly]
    private readonly IFanslyRules _fansly;

    protected override string ServiceSymbol { get => "SUBLINK_FANSLY"; }

    protected override string[] ServiceUsings {
        get => new string[] {
            "xyz.yewnyx.SubLink.Fansly",
            "xyz.yewnyx.SubLink.Fansly.Events"
        };
    }

    protected override string[] ServiceAssemblies {
        get => Array.Empty<string>();
    }

    public CompilerService(ILogger logger, FanslyGlobals globals, IFanslyRules fansly) : base(logger, globals) {
        _fansly = fansly;
        globals.fansly = _fansly;
        globals.logger = logger;
    }
}