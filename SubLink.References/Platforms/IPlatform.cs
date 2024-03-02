using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.Platforms;

public interface IPlatformRules { }

public interface IPlatform {
    // Useful for enumerating the loaded platforms
    string GetPlatformName();
    string GetServiceSymbol();
    string[] GetAdditionalUsings();
    string[] GetAdditionalAssemblies();

    void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder);
    void ConfigureServices(HostBuilderContext context, IServiceCollection services);

    void AppendRules(Dictionary<string, IPlatformRules> rules);

    void SetLogger(ILogger logger);

    // Let the interface handle this, no reflection overhead
    Task StartServiceAsync();
    Task StopServiceAsync();
}
