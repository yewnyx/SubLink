﻿using Microsoft.Extensions.Configuration;
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
    internal static string PlatformConfigFile = Path.Combine("settings", $"{PlatformName}.json");

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles
    private ILogger? _logger { get; set; }
    private IServiceProvider? _serviceProvider { get; set; }
    private StreamPadService? _service { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore IDE0052 // Remove unread private members

    // Useful for enumerating the loaded platforms
    public string GetPlatformName() => PlatformName;
    public string GetServiceSymbol() => "SUBLINK_STREAMPAD";
    public string[] GetAdditionalUsings() => [
        "xyz.yewnyx.SubLink.Streampad.Services",
    ];
    public string[] GetAdditionalAssemblies() => [];

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
            .Configure<StreamPadSettings>(context.Configuration.GetSection(PlatformName))
            .AddScoped<StreamPadRules>()
            .AddScoped<StreamPadService>();
    }

    public void AppendRules(Dictionary<string, IPlatformRules> rules) {
        var rulesSvc = _serviceProvider?.GetService<StreamPadRules>();

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
        if (_service != null)
            await _service.StopAsync();

        _service = _serviceProvider?.GetService<StreamPadService>();

        if (_service != null)
            await _service.StartAsync();
    }
    public async Task StopServiceAsync() {
        if (_service == null)
            return;

        await _service.StopAsync();
        _service = null;
    }
}
