using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Fansly.Services;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.Fansly;

public class Platform : IPlatform {
    internal const string PlatformName = "Fansly";
    internal const string PlatformConfigFile = "settings.Fansly.json";

    private ILogger _logger { get; set; }
    private IServiceProvider _serviceProvider { get; set; }

    private FanslyService? service { get; set; }

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => PlatformName;
    public string GetServiceSymbol() => "SUBLINK_FANSLY";
    public string[] GetAdditionalUsings() => Array.Empty<string>();
    public string[] GetAdditionalAssemblies() => Array.Empty<string>();

    public bool EnsureConfigExists() {
        if (!File.Exists(PlatformConfigFile)) {
            File.WriteAllText(PlatformConfigFile, """
{
  "Fansly": {
    "Token": "",
    "Username": ""
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
            .Configure<FanslySettings>(context.Configuration.GetSection("Fansly"))
            .AddScoped<FanslyRules>()
            .AddScoped<FanslyService>();
    }

    public void AppendRules(Dictionary<string, IPlatformRules> rules) {
        var rulesSvc = _serviceProvider.GetService<FanslyRules>();

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

        service = _serviceProvider.GetService<FanslyService>();

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
