using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx {
    public sealed class CompilerService : BaseCompilerService<TwitchGlobals> {
        [UsedImplicitly]
        private readonly ITwitchRules _twitch;

        protected override string ServiceSymbol { get => "SUBLINK_TWITCH"; }

        protected override string[] ServiceUsings {
            get => new string[] {
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
            };
        }

        protected override string[] ServiceAssemblies {
            get => new string[] {
                "TwitchLib.Api",
                "TwitchLib.Api.Core",
                "TwitchLib.Api.Core.Enums",
                "TwitchLib.Api.Core.Interfaces",
                "TwitchLib.Api.Core.Models",
                "TwitchLib.Api.Helix",
                "TwitchLib.Api.Helix.Models",
                "TwitchLib.Client",
                "TwitchLib.Client.Enums",
                "TwitchLib.Client.Models",
                "TwitchLib.Communication",
                "TwitchLib.EventSub.Core",
                "TwitchLib.EventSub.Websockets",
                "TwitchLib.PubSub",
            };
        }

        public CompilerService(ILogger logger, TwitchGlobals globals, ITwitchRules twitch) : base(logger, globals) {
            _twitch = twitch;
            globals.twitch = _twitch;
            globals.logger = logger;
        }
    }
}