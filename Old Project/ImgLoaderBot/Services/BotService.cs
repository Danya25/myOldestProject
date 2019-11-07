using ConsoleApp2.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp2.Services
{
    internal sealed class BotService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IpicUploadService _uploadService;
        private readonly IOptions<BotOptions> _botOptions;
        private readonly ILogger<BotService> _logger;

        public BotService(ITelegramBotClient botClient,
            IpicUploadService uploadService,
            IOptions<BotOptions> botOptions,
            ILogger<BotService> logger)
        {
            _botClient = botClient;
            _uploadService = uploadService;
            _botOptions = botOptions;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var updateReceiver = new QueuedUpdateReceiver(_botClient);
            updateReceiver.StartReceiving(_botOptions.Value.AllowedUpdates, cancellationToken: stoppingToken);

            await foreach (var update in updateReceiver.YieldUpdatesAsync().WithCancellation(stoppingToken))
            {
                _logger.LogDebug("Message with type {0} received from chat {1}", update.Message.Type, update.Message.Chat.Id);

                var photo = update.Message.Photo?[^1];
                if (photo == null)
                    continue;

                var uploadResult = await _uploadService.UploadImageToIpicAsync(photo.FileId, stoppingToken);

                _logger.LogDebug("Upload result for image {0} url is {1}", uploadResult.FileName, uploadResult.FileUrl);

                try
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat, $"[{uploadResult.FileName}]({uploadResult.FileUrl})", ParseMode.Markdown);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while sending message to chat {0}", update.Message.Chat.Id);
                }
            }
        }
    }
}
