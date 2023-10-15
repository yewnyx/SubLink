using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx {
    public class CompilerService : BaseCompilerService<KickGlobals> {
        [UsedImplicitly]
        private readonly IKickRules _kick;

        protected override string ServiceSymbol { get => "SUBLINK_KICK"; }

        protected override string[] ServiceUsings {
            get => new string[] {
                "xyz.yewnyx.SubLink.Kick",
                "xyz.yewnyx.SubLink.Kick.Events",
            };
        }

        protected override string[] ServiceAssemblies {
            get => new string[] {
            };
        }

        public CompilerService(ILogger logger, KickGlobals globals, IKickRules kick) : base(logger, globals) {
            _kick = kick;
            globals.kick = _kick;
            globals.logger = logger;
        }
    }
}