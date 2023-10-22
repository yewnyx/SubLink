using System.Text.RegularExpressions;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace tech.sublink.SubLinkConsole;

internal partial class Program {
    public static async Task Main(string[] args) {
        var program = new Program();
        await program.Run(args);
    }

    IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureServices((context, services) => {
                services
                    .Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true)
                    .AddSingleton<ConsoleLogger>()
                    .AddHostedService<SubLinkService>();
            })
            .UseSerilog((context, configuration) => {
                const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {Message:l}{NewLine}{Exception}";
                configuration
                    .MinimumLevel.Verbose()
                    .WriteTo.File("log/log_.txt", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day)
                    .WriteTo.Console(outputTemplate: outputTemplate, restrictedToMinimumLevel: LogEventLevel.Information);

                configuration
                    .Enrich.FromLogContext()
                    .Enrich.FromGlobalLogContext();
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
        var programName = FiggleFonts.Slant.Render("SubLink Console");
        programName = ProgramNameRegex().Replace(programName, string.Empty);
        Console.Write(programName);
        Console.WriteLine("----------------------------------------------------------------");

        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();
        await host.WaitForShutdownAsync();
    }

    [GeneratedRegex(@"^\s+$[\r\n]*", RegexOptions.Multiline)]
    private static partial Regex ProgramNameRegex();
}
