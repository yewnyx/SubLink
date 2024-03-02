using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.Kick;

public class Platform : IPlatform {
    private ILogger _logger = default;

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => "Kick";
    public string GetServiceSymbol() => "SUBLINK_KICK";
    public string[] GetAdditionalUsings() => Array.Empty<string>();
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
        await Task.CompletedTask;
    }
    public async Task StopServiceAsync() {
        await Task.CompletedTask;
    }
}
