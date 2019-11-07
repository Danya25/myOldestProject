using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MongoDB;
using Newtonsoft.Json;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model.In;
using Qiwi.BillPayments.Model;
using System.Threading;

namespace BotForSale
{
   public class Program
    {
        
        public static string[] id;

        public  static ITelegramBotClient botClient;
        public static void Main()
        {

            botClient = new TelegramBotClient("API_KEY");
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(me.FirstName+": Бот готов к работе!");

            botClient.OnMessage += BotClient_OnMessage;
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;

            botClient.StartReceiving();
            Console.Read();
        }
        public static void StopReceiving()
                {
                    botClient.StopReceiving();
                }

        private async static void BotClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName}";
            Console.WriteLine($"{name} нажал на кнопку {buttonText} ПО времени {DateTime.Now}");
            MainWork mongoDB = new MainWork();
            switch (buttonText)
            {
                case "NORD":  
                    var words1 = mongoDB.GetInformCS("NORD").Result;
                    if(words1.Count > 0)
                    {
                        foreach(string word in words1)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "IPVANISH":
                    var words = mongoDB.GetInformCS("IPVANISH").Result;
                    if (words.Count > 0)
                    {
                        foreach (string word in words)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "ZETMATE":
                    var words2 = mongoDB.GetInformCS("ZETMATE").Result;
                    if (words2.Count > 0)
                    {
                        foreach (string word in words2)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "HMAPRO":
                    var words3 = mongoDB.GetInformCS("HMAPRO").Result;
                    if (words3.Count > 0)
                    {
                        foreach (string word in words3)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "WINDSCRIBE":
                    var words4 = mongoDB.GetInformCS("WINDSCRIBE").Result;
                    if (words4.Count > 0)
                    {
                        foreach (string word in words4)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "TUNNELBEAR":
                    var words5 = mongoDB.GetInformCS("TUNNELBEAR").Result;
                    if (words5.Count > 0)
                    {
                        foreach (string word in words5)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
                case "VYPRVPN":
                    var words6 = mongoDB.GetInformCS("VYPRVPN").Result;
                    if (words6.Count > 0)
                    {
                        foreach (string word in words6)
                        {
                            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, word);
                        }
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для покупки введите /buy и ID через ',' без пробелов");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "Аккаунтов в данной категории нет!");
                    }
                    break;
            }
            try
            {
                await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
            }
            catch
            {
                Console.WriteLine("Был возможный краш!");
            }
        }


        private async static void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type != MessageType.Text)
                return;
            Console.WriteLine($"{message.From.FirstName} отправил сообщение: {message.Text} ПО времени {DateTime.Now}");

            if (message.Text.StartsWith("/buy"))
            {
                //////////////////////////PAY//////////////////////
                id = message.Text.Replace("/buy", string.Empty).TrimStart().Split(',');
                MainWork newWork = new MainWork();
                BDwithPay pay = new BDwithPay();
                var boolInfo = newWork.CheckInDBAcc(id).GetAwaiter().GetResult();
                if(boolInfo.All(x=>x == true))
                {
                    var cost = newWork.CostsID(id); // цена акков по ID
                    int IdPeop = e.Message.From.Id; // ID человека
                    string FirstName = e.Message.From.FirstName; // Ник юзера
                    string LastName = e.Message.From.LastName; // ник юзера
                    string url = pay.Method(IdPeop, FirstName, LastName,id).GetAwaiter().GetResult();
                    //сделать проверку на наличие акков
                    await botClient.SendTextMessageAsync(e.Message.Chat, "На покупку у вас будет 5 минут, после ваш счет будет отменен");
                    await botClient.SendTextMessageAsync(e.Message.Chat, url);
                    if (pay.UpdatePayments(id).GetAwaiter().GetResult() == true)
                    {
                      var accsForGive = newWork.BuyId(id).GetAwaiter().GetResult();
                      foreach(var acc in accsForGive)
                      {
                            await botClient.SendTextMessageAsync(e.Message.Chat, acc);
                      }
                        await botClient.SendTextMessageAsync(e.Message.Chat, "Спасибо за покупку, покупайте еще!");
                        Console.WriteLine($"Была совершена покупка: FirstName: {e.Message.From.FirstName} || LastName: {e.Message.From.LastName}");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, "Вы не успели оплатить. Ваша заявка отклонена!");
                        Console.WriteLine($"Не успел оплатить за аккаунты: FirstName:{e.Message.From.FirstName} LastName:{e.Message.From.LastName}");
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, "Один или несколько аккаунтов из вашего списка уже КУПЛЕНЫ либо вы ошиблись с вводом ID!");
                    Console.WriteLine($"Нехватка Акков/ОшибкаID:\nFirstName:{e.Message.From.FirstName}\nLastName:{e.Message.From.LastName} ");
                }
                //////////////////////////PAY//////////////////////
            }

            switch (message.Text)
            {
                case "/start":
                    try
                    {
                        string commands = "Создатель TG: /creator \r\nПокупка: /menu \r\nДополнительная информация: /info";
                        var replyKeyBoard = new ReplyKeyboardMarkup(new[]{
                                new[]
                                {
                                    new KeyboardButton("MENU")
                                }
                            })
                        { ResizeKeyboard = true };
                        await botClient.SendTextMessageAsync(e.Message.Chat, commands, replyMarkup: replyKeyBoard);
                    }
                    catch
                    {
                        Console.WriteLine("Был возможный краш!");
                    }
                    break;
                case "/menu":
                case "MENU":
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, "VPN сервисы:");
                        var inlineKeyBoard = new InlineKeyboardMarkup(new[] {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("NORD")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("IPVANISH")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("ZETMATE")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("HMAPRO")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("WINDSCRIBE")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("TUNNELBEAR")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("VYPRVPN")
                                }
                            });
                        await botClient.SendTextMessageAsync(e.Message.Chat, "Выберите что хотели бы купить", replyMarkup: inlineKeyBoard);
                    }
                    catch
                    {
                        Console.WriteLine("Был возможный краш!");
                    }
                    break;
                case "/creator":
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, "@user1771");
                    }
                    catch
                    {
                        Console.WriteLine("Был возможный краш!");
                    }
                    
                    break;
                case "/info":
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, "Для поддержки обращаться - @user1771");
                    }
                    catch
                    {
                        Console.WriteLine("Был возможный краш!");
                    }
                    break;

            }
        }
    }
}
