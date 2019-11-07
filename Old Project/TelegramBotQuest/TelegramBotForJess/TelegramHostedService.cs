using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TelegramBotForJess
{
    internal sealed class TelegramHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUpdateHandler _handler;
        private readonly ILogger<TelegramHostedService> _logger;

        public TelegramHostedService(ITelegramBotClient client, IUpdateHandler handler, ILogger<TelegramHostedService> logger)
        {
            _botClient = client;
            _handler = handler;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var me = await _botClient.GetMeAsync();
                Console.WriteLine(me.FirstName + ": Бот готов к работе!");

                await _botClient.ReceiveAsync(_handler, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while receiving updates");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
