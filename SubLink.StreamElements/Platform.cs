using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;
using xyz.yewnyx.SubLink.StreamElements.Services;

namespace xyz.yewnyx.SubLink.StreamElements;

public class Platform : IPlatform {
    internal const string PlatformName = "StreamElements";
    internal const string PlatformConfigFile = "settings.StreamElements.json";

    private ILogger _logger { get; set; }
    private IServiceProvider _serviceProvider { get; set; }

    private StreamElementsService? service { get; set; }

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => PlatformName;
    public string GetServiceSymbol() => "SUBLINK_STREAMELEMENTS";
    public string[] GetAdditionalUsings() => Array.Empty<string>();
    public string[] GetAdditionalAssemblies() => Array.Empty<string>();

    public bool EnsureConfigExists() {
        if (!File.Exists(PlatformConfigFile)) {
            File.WriteAllText(PlatformConfigFile, """
{
  "StreamElements": {
    "JWTToken": ""
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
            .Configure<StreamElementsSettings>(context.Configuration.GetSection("StreamElements"))
            .AddScoped<StreamElementsRules>()
            .AddScoped<StreamElementsService>();
    }

    public void AppendRules(Dictionary<string, IPlatformRules> rules) {
        var rulesSvc = _serviceProvider.GetService<StreamElementsRules>();

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

        service = _serviceProvider.GetService<StreamElementsService>();

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
