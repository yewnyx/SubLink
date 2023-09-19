﻿using System;
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
using TwitchLib.EventSub.Websockets.Extensions;

namespace xyz.yewnyx.SubLink;

internal class Program {
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
  "Discord": {
    "Webhook": ""
  },
  "StreamPad": {
    "WebSocketUrl": "",
    "ChannelId": "",
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
    
    IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureAppConfiguration((context, builder) => {
                builder.AddJsonFile("settings.json", false, true);
            })
            .ConfigureServices((context, services) => {
                services
                    .Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true)
                    .Configure<TwitchSettings>(context.Configuration.GetSection("Twitch"))
                    .Configure<DiscordSettings>(context.Configuration.GetSection("Discord"))
                    .Configure<StreamPadSettings>(context.Configuration.GetSection("StreamPad"))
                    .Configure<SubLinkSettings>(context.Configuration.GetSection("SubLink"))
                    .AddTwitchLibEventSubWebsockets()
                    .AddHostedService<SubLinkService>()
                    .AddScoped<IRules, Rules>()
                    .AddScoped<ITwitchRules, TwitchRules>()
                    .AddScoped<TwitchService>()
                    .AddScoped<StreamPadService>()
                    .AddScoped<CompilerService>();
            })
            .UseSerilog((context, configuration) => {
                var webhook = context.Configuration.GetSection("Discord").Get<DiscordSettings>();

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
        var programName = FiggleFonts.Slant.Render("SubLink");
        programName = Regex.Replace(programName, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        Console.Write(programName);
        Console.WriteLine(@"by
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
\____/\__,_/\__/   \____/_/_/  /_/  /_____/\__,_/\__,_/_/\___/");
        
        using (var host = CreateHostBuilder(args).Build()) {
            // Start Sublink service, conditionally starting TwitchService and/or StreamPadService
            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }
    }
}