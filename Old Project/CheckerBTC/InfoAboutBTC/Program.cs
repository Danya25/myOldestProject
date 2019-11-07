using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace InfoAboutBTC
{
    class Program
    {
        public static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("825813442:AAFFSnDW-u1JmL0JsMMOAZ8V4qCU_kNOVz0");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"{me.FirstName}: готов к работе!");

            botClient.OnMessage += BotClient_OnMessage;

            botClient.StartReceiving();
            Console.ReadKey();
        }

        private async static void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            var replyKeyBoard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Список всех монет!")
                }
            })
            { ResizeKeyboard = true };
            
            var message = e.Message;
            if (e.Message.Type != MessageType.Text)
                return;
            Console.WriteLine(e.Message.From.FirstName, $" отправил сообщение: {e.Message.Text}");
            try
            {
                var info = GetInfoAboutCrypt(message.Text, e);
                await botClient.SendTextMessageAsync(e.Message.Chat, info);
            }
            catch
            {

            }
            if(e.Message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(e.Message.Chat, "Запуск!", replyMarkup: replyKeyBoard);
            }
            else if(e.Message.Text == "Список всех монет!")
            {
                using(var file = File.OpenRead("list.txt"))
                {
                    await botClient.SendDocumentAsync(e.Message.Chat, new InputOnlineFile(file, "list.txt"));
                }
            }
        }
        
        public static string GetInfoAboutCrypt(string nameCrypt, MessageEventArgs e)
        {
            var allCoins = File.ReadAllLines("list.txt");
            var infoString = new StringBuilder();

            if (allCoins.Any(coin => coin.Equals(nameCrypt, StringComparison.OrdinalIgnoreCase)))
            {
                using (var wc = new WebClient())
                {
                    var infoAboutCrypt = wc.DownloadString($"https://min-api.cryptocompare.com/data/pricemulti?fsyms={nameCrypt.ToUpper()}&tsyms=BTC,USD,EUR,RUB&api_key=7b7e0ec981482a3ab11afc5cfc6d25fc974882e1ef9fc636d2b0d549e64afd59");
                    var info = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(infoAboutCrypt);
                    var i = 0;
                    foreach (var infoItem in info)
                    {
                        infoString.Append(string.Join(" | ", infoItem.Value.Select(it => $"{it.Key}: {it.Value}")));

                        if (++i != info.Count)
                            infoString.Append(Environment.NewLine);
                    }
                    return infoString.ToString();
                }
            }
            return null;
        }

    }
    public class CryptoCurrencyInfo
    {
        public Dictionary<string, decimal> BTC { get; set; }
    }
}
