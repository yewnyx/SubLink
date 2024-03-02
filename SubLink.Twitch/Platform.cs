using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.Twitch;

public class Platform : IPlatform {
    private ILogger _logger = default;

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => "Twitch";
    public string GetServiceSymbol() => "SUBLINK_TWITCH";
    public string[] GetAdditionalUsings() => new[]{
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
    public string[] GetAdditionalAssemblies() => Array.Empty<string>();

    public void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder) {

    }

    public void ConfigureServices(HostBuilderContext context, IServiceCollection services) {

    }

    public void AppendRules(Dictionary<string, IPlatformRules> rules) {

    }

    public void SetLogger(ILogger logger) {
        _logger = logger;
    }

    // Let the interface handle this, no reflection overhead
    public async Task StartServiceAsync() {
        _logger.Information("[{TAG}] Async Start called", "Twitch");
        await Task.CompletedTask;
    }

    public async Task StopServiceAsync() {
        _logger.Information("[{TAG}] Async Stop called", "Twitch");
        await Task.CompletedTask;
    }
}
