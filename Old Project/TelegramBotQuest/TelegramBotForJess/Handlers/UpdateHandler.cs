using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForJess.Data;

namespace TelegramBotForJess.Handlers
{
    internal sealed partial class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ITelegramBotClient _client;
        private readonly DataContext _context;

        public UpdateHandler(ILogger<UpdateHandler> logger, ITelegramBotClient client, DataContext context)
        {
            _logger = logger;
            _client = client;
            _context = context;
        }

        public UpdateType[] AllowedUpdates => new[] { UpdateType.Message };

        public Task HandleError(Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Error occurred while sending requests to Telegram servers");
            return Task.CompletedTask;
        }

        public async Task HandleUpdate(Update update, CancellationToken cancellationToken)
        {
            try
            {
                await (update switch
                {
                    { Message: { } m } => HandleMessage(m, cancellationToken),
                    _ => Task.CompletedTask
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing update");
            }
        }
    }
}
