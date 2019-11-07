using Telegram.Bot.Types.Enums;

namespace ConsoleApp2.Options
{
    internal sealed class BotOptions
    {
        public string Token { get; set; } = string.Empty;

        public UpdateType[]? AllowedUpdates { get; set; }
    }
}
