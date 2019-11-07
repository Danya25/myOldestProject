using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using TelegramBotForJess.Data;
using TelegramBotForJess.Handlers;
using TelegramBotForJess.Options;

namespace TelegramBotForJess
{
    internal static class Program
    {
        private static async Task Main()
        {
            var host = GetHost();
            await host.RunAsync();
        }


        private static IHost GetHost() =>
            new HostBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<DataOptions>(context.Configuration.GetSection(nameof(DataOptions)));
                    services.AddSingleton<DataContext>();

                    var botOptions = new BotOptions();
                    context.Configuration.GetSection(nameof(BotOptions)).Bind(botOptions);

                    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botOptions.Token));
                    services.AddSingleton<IUpdateHandler, UpdateHandler>();
                    services.AddHostedService<TelegramHostedService>();
                })
                .Build();
    }
}
