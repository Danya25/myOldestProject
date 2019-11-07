using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp2.Extensions;
using ConsoleApp2.Options;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
           await new HostBuilder()
                .ConfigureLogging(builder =>
                {
                    builder.AddDebug();
                    builder.AddConsole();
                    builder.AddEventSourceLogger();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .UseEnvironment(Environments.Development)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices(services =>
                {
                    services.Configure<BotOptions>(options =>
                    {
                        options.Token = "API_KEY";
                        options.AllowedUpdates = new[] { UpdateType.Message };
                    });

                    services.AddGeneralServices();
                })
                .RunConsoleAsync();
        }
    }
}