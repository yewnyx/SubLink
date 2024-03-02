using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;
using xyz.yewnyx.SubLink.Streampad.Services;

namespace xyz.yewnyx.SubLink.Streampad;

public class Platform : IPlatform {
    internal const string PlatformName = "StreamPad";
    internal const string PlatformConfigFile = "settings.StreamPad.json";

    private ILogger _logger { get; set; }
    private IServiceProvider _serviceProvider { get; set; }

    private StreamPadService? service { get; set; }

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => PlatformName;
    public string GetServiceSymbol() => "SUBLINK_STREAMPAD";
    public string[] GetAdditionalUsings() => new[]{
        "xyz.yewnyx.SubLink.Streampad.Services",
    };
    public string[] GetAdditionalAssemblies() => Array.Empty<string>();

    public bool EnsureConfigExists() {
        if (!File.Exists(PlatformConfigFile)) {
            File.WriteAllText(PlatformConfigFile, """
{
  "StreamPad": {
    "WebSocketUrl": "",
    "ChannelId": ""
  }
}
""");
            return false;
        }

        return true;
    }

    public void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder) =>
        builder.AddJsonFile(PlatformConfigFile, false, true);

    public void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
        services
            .Configure<StreamPadSettings>(context.Configuration.GetSection("StreamPad"))
            .AddScoped<StreamPadRules>()
            .AddScoped<StreamPadService>();
    }

    public void AppendRules(Dictionary<string, IPlatformRules> rules) {
        var rulesSvc = _serviceProvider.GetService<StreamPadRules>();

        if (rulesSvc != null)
            rules.Add(PlatformName, rulesSvc);
    }

    public void SetLogger(ILogger logger) {
        _logger = logger;
    }

    public void SetServiceProvider(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    // Let the interface handle this, no reflection overhead
    public async Task StartServiceAsync() {
        if (service != null)
            await service.StopAsync();

        service = _serviceProvider.GetService<StreamPadService>();

        if (service != null)
            await service.StartAsync();
    }
    public async Task StopServiceAsync() {
        if (service == null)
            return;

        await service.StopAsync();
        service = null;
    }
}
