using System.Reflection;
using System.Text.RegularExpressions;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using xyz.yewnyx.SubLink.Platforms;
using xyz.yewnyx.SubLink.Services;

namespace xyz.yewnyx.SubLink;


internal partial class Program {
    public static async Task Main(string[] args) {
        if (!File.Exists("settings.json")) {
            var discriminator = new Random().Next(1, 9999);
            var settingsTemplate = """
{
  "Discord": {
    "Webhook": ""
  },
  "SubLink": {
    "Discriminator": {discriminator}
  }
}
""";
            settingsTemplate = settingsTemplate.Replace("{discriminator}", $"{discriminator}");
            File.WriteAllText("settings.json", settingsTemplate);
        }

        var program = new Program();
        await program.Run(args);
    }

    private string _exeDirectory = string.Empty;

    IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureAppConfiguration((context, builder) => {
                builder.AddJsonFile("settings.json", false, true);

                // Run IPlatform.ConfigureAppConfiguration(context, services); for every platform
                foreach (var platform in HostGlobals.Platforms.Values) {
                    platform.Entry.ConfigureAppConfiguration(context, builder);
                }
            })
            .ConfigureServices((context, services) => {
                services
                    .Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true)
                    .Configure<DiscordSettings>(context.Configuration.GetSection("Discord"))
                    .Configure<SubLinkSettings>(context.Configuration.GetSection("SubLink"))
                    .AddSingleton<ScriptGlobals>()
                    .AddHostedService<SubLinkService>()
                    .AddScoped<OSCSupportService>()
                    .AddScoped<CompilerService>();

                // Run IPlatform.ConfigureServices(context, services); for every platform
                foreach (var platform in HostGlobals.Platforms.Values) {
                    platform.Entry.ConfigureServices(context, services);
                }
            })
            .UseSerilog((context, configuration) => {
                var webhook = context.Configuration.GetSection("Discord").Get<DiscordSettings>();

                if (webhook == null)
                    return;

                const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {Message:l}{NewLine}{Exception}";

                configuration
                    .MinimumLevel.Verbose()
                    .WriteTo.File("log/log_.txt", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day)
                    .WriteTo.Console(outputTemplate: outputTemplate,
                        restrictedToMinimumLevel: LogEventLevel.Information);

                /*
                if (!string.IsNullOrWhiteSpace(webhook.WebhookToken) && webhook.WebhookId != 0) {
                    configuration.WriteTo.Async(a =>
                        a.Discord(webhook.WebhookId, webhook.WebhookToken, restrictedToMinimumLevel: LogEventLevel.Information));
                }
                */

                configuration
                    .Enrich.FromLogContext()
                    .Enrich.FromGlobalLogContext();

                var subLinkSettings = context.Configuration.GetSection("SubLink").Get<SubLinkSettings>();
                using (GlobalLogContext.Lock()) {
                    GlobalLogContext.PushProperty("Discriminator", subLinkSettings?.Discriminator);
                }
            });
    }

    public async Task Run(string[] args) {
        Console.WriteLine(@"
----------------------------Credits-----------------------------
__  __
\ \/ /__ _      ______  __  ___  __
 \  / _ \ | /| / / __ \/ / / / |/_/
 / /  __/ |/ |/ / / / / /_/ />  <
/_/\___/|__/|__/_/ /_/\__, /_/|_|
and                  /____/
   ______      __     _______      __   ______    __    ___
  / ____/___ _/ /_   / ____(_)____/ /  / ____/___/ /___/ (_)__
 / /   / __ `/ __/  / / __/ / ___/ /  / __/ / __  / __  / / _ \
/ /___/ /_/ / /_   / /_/ / / /  / /  / /___/ /_/ / /_/ / /  __/
\____/\__,_/\__/   \____/_/_/  /_/  /_____/\__,_/\__,_/_/\___/
and __                           ____              _
   / /   ____ ___  ___________ _/ __ \____  ____  (_)__  _____
  / /   / __ `/ / / / ___/ __ `/ /_/ / __ \/_  / / / _ \/ ___/
 / /___/ /_/ / /_/ / /  / /_/ / _, _/ /_/ / / /_/ /  __/ /
/_____/\__,_/\__,_/_/   \__,_/_/ |_|\____/ /___/_/\___/_/
----------------------------Starting----------------------------");
        var programName = FiggleFonts.Slant.Render("SubLink");
        programName = ProgramNameRegex().Replace(programName, string.Empty);
        Console.Write(programName);
        Console.WriteLine("----------------------------------------------------------------");

        _exeDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? AppDomain.CurrentDomain.BaseDirectory;
        var platformIfaceType = typeof(IPlatform);

        foreach (var platformLib in Directory.GetFiles(Path.Combine(_exeDirectory, "Platforms"), "*.dll")) {
            var libAsm = Assembly.LoadFile(platformLib);
            var platformEntryType = libAsm.GetExportedTypes()
                .FirstOrDefault(t => platformIfaceType.IsAssignableFrom(t));

            if (platformEntryType == null || platformEntryType == default)
                continue;

            var platformEntry = (IPlatform?)Activator.CreateInstance(platformEntryType);

            if (platformEntry == null)
                continue;

            HostGlobals.Platforms.Add(platformEntry.GetPlatformName(), new() {
                Assembly = libAsm,
                Entry = platformEntry
            });
        }

        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();
        await host.WaitForShutdownAsync();
    }

    [GeneratedRegex(@"^\s+$[\r\n]*", RegexOptions.Multiline)]
    private static partial Regex ProgramNameRegex();
}
