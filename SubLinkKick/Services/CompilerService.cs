using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx {
    public class CompilerService : BaseCompilerService {
        [UsedImplicitly]
        private readonly IKickRules _kick;

        protected override Type GlobalsType { get => typeof(Globals); }

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

        public CompilerService(ILogger logger, IKickRules kick) : base(logger) {
            _kick = kick;
            Globals.kick = _kick;
            CommonGlobals.logger = logger;
        }
    }
}