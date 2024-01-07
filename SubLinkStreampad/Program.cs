using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.Discord;

namespace xyz.yewnyx.SubLink;

internal partial class Program {
    public static async Task Main(string[] args) {
        if (!File.Exists("settings.json")) {
            var discriminator = new Random().Next(1, 9999);

            var settingsTemplate = """
{
  "Twitch": {
    "ClientId": "",
    "ClientSecret": "",
    "AccessToken": "",
    "RefreshToken": "",
    "Scopes": [
      "bits:read",
      "channel:manage:polls",
      "channel:manage:redemptions",
      "channel:read:hype_train",
      "channel:read:polls",
      "channel:read:redemptions",
      "channel:read:subscriptions",
      "channel:read:vips",
      "chat:edit",
      "chat:read"
    ]
  },
  "Kick": {
    "PusherKey": "",
    "PusherCluster": "",
    "ChatroomId": ""
  },
  "StreamPad": {
    "WebSocketUrl": "",
    "ChannelId": ""
  },
  "StreamElements": {
    "JWTToken": ""
  },
  "Fansly": {
    "Token": "",
    "Username": ""
  },
  "Discord": {
    "Webhook": ""
  },
  "SubLink": {
    "Discriminator": {discriminator},
    "OscIPAddress": "127.0.0.1",
    "OscPort": 9000,
    "ScriptName": "SubLink.cs"
  }
}
""";
            settingsTemplate = settingsTemplate.Replace("{discriminator}", $"{discriminator}");
            File.WriteAllText("settings.json", settingsTemplate);
        }

        var program = new Program();
        await program.Run(args);
    }

    IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureAppConfiguration((context, builder) => {
                builder.AddJsonFile("settings.json", false, true);
            })
            .ConfigureServices((context, services) => {
                services
                    .Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true)
                    .Configure<StreamPadSettings>(context.Configuration.GetSection("StreamPad"))
                    .Configure<DiscordSettings>(context.Configuration.GetSection("Discord"))
                    .Configure<SubLinkSettings>(context.Configuration.GetSection("SubLink"))
                    .AddSingleton<StreampadGlobals>()
                    .AddHostedService<SubLinkService<StreampadGlobals, CompilerService, StreamPadService>>()
                    .AddScoped<OSCSupportService<StreampadGlobals>>()
                    .AddScoped<IStreamPadRules, StreamPadRules>()
                    .AddScoped<StreamPadService>()
                    .AddScoped<CompilerService>();
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

                if (!string.IsNullOrWhiteSpace(webhook.WebhookToken) && webhook.WebhookId != 0) {
                    configuration.WriteTo.Async(a =>
                        a.Discord(webhook.WebhookId, webhook.WebhookToken, restrictedToMinimumLevel: LogEventLevel.Information));
                }

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
        var programName = FiggleFonts.Slant.Render("SubLinkStreamPad");
        programName = ProgramNameRegex().Replace(programName, string.Empty);
        Console.Write(programName);
        Console.WriteLine("----------------------------------------------------------------");
        using (var host = CreateHostBuilder(args).Build()) {
            var ks = host.Services.GetService<IOptions<StreamPadSettings>>();

            if (
                string.IsNullOrWhiteSpace(ks!.Value.ChannelId) ||
                string.IsNullOrWhiteSpace(ks!.Value.WebSocketUrl)
            ) {
                Console.WriteLine("Your StreamPad settings are set up incorrectly.");
                return;
            }

            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }
    }

    [GeneratedRegex(@"^\s+$[\r\n]*", RegexOptions.Multiline)]
    private static partial Regex ProgramNameRegex();
}