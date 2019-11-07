using ConsoleApp2.Options;
using ConsoleApp2.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;

namespace ConsoleApp2.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<IpicParseService>();
            services.AddSingleton<IpicUploadService>();

            services.AddSingleton<ITelegramBotClient>(serviceProvider =>
            {
                var botOptions = serviceProvider.GetRequiredService<IOptions<BotOptions>>();
                return new TelegramBotClient(botOptions.Value.Token);
            });

            services.AddHostedService<BotService>();

            services.AddHttpClient<IpicUploadService>(client =>
            {
                client.BaseAddress = new Uri("http://ipic.su/");
            });
            services.AddScoped<HtmlDocument>();
        }
    }
}
