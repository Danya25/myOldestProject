using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UserM = TelegramBotForJess.Models.User;

namespace TelegramBotForJess.Handlers
{
    internal partial class UpdateHandler
    {
        private async Task HandleMessage(Message m, CancellationToken ct)
        {
            var idTelegram = m.From.Id;

            var status = await _context.UserColl.FindOneAndUpdateAsync(sp => sp.IdTelegram == idTelegram,
                Builders<UserM>.Update.SetOnInsert(u => u.Status, "start"),
                new FindOneAndUpdateOptions<UserM, string>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After,
                    Projection = Builders<UserM>.Projection.Expression(u => u.Status)
                },
                ct);

            if (status == "done")
            {
                await _client.SendTextMessageAsync(m.Chat, "Заявку отправлять можно только 1 раз!");
                return;
            }

            _logger.LogDebug($"{m.From.FirstName} отправил сообщение: {m.Text} ПО времени {DateTime.Now}");

            switch (m)
            {
                case { Text: { } } when status == "start":
                    {
                        await _client.SendTextMessageAsync(m.Chat, "Здравствуйте, бот для заявок  в закрытый чат селлеров."+
            "Для вступления ответьте на все следующие вопросы максимально подробно.", cancellationToken: ct);
                        await _client.SendTextMessageAsync(m.Chat, "1) Предоставьте ссылки на ваши форумные профили (если таковые имеются)(в одном сообщение):", cancellationToken: ct);
                        var t1 = _context.UserColl.UpdateOneAsync(u => u.IdTelegram == idTelegram,
                                Builders<UserM>.Update.Set(u => u.Status, "first"),
                                cancellationToken: ct);
                        break;
                    }

                case { Text: { } t } when status == "first":
                    {
                        var t1 = _context.UserColl.UpdateOneAsync(u => u.IdTelegram == idTelegram,
                            Builders<UserM>.Update.Set(u => u.Status, "second").Push(u => u.Answers, t),
                            cancellationToken: ct);
                        var t2 = _client.SendTextMessageAsync(m.Chat, "Назовите чаты, где вы сидите:", cancellationToken: ct);
                        await Task.WhenAll(t1, t2);
                        break;
                    }


                case { Text: { } t } when status == "second":
                    {
                        var t1 = _context.UserColl.UpdateOneAsync(u => u.IdTelegram == idTelegram,
                            Builders<UserM>.Update.Set(u => u.Status, "third").Push(u => u.Answers, t),
                            cancellationToken: ct);
                        var t2 = _client.SendTextMessageAsync(m.Chat, "Укажите ваш род деятельности (если вы селлер, напишите об этом)(в одном сообщение):", cancellationToken: ct);
                        await Task.WhenAll(t1, t2);
                        break;
                    }

                case { Text: { } t } when status == "third":
                    {
                        var t1 = _context.UserColl.FindOneAndUpdateAsync(u => u.IdTelegram == idTelegram,
                            Builders<UserM>.Update.Set(u => u.Status, "done").Push(u => u.Answers, t).Set(u => u.IsAnswer, true),
                            new FindOneAndUpdateOptions<UserM, List<string>>
                            {
                                ReturnDocument = ReturnDocument.After,
                                Projection = Builders<UserM>.Projection.Expression(u => u.Answers)
                            },
                            ct);
                        var t2 = _client.SendTextMessageAsync(m.Chat, "Мы приняли вашу заявку, в скором времени Мы напишем Вам.", cancellationToken: ct);

                        var text = string.Join('\n', t1.Result.Select((it, i) => $"<b>{i + 1}.</b> {HtmlEncoder.Default.Encode(it)}")
                            .Prepend($"<a href=\"tg://user?id={idTelegram}\">{idTelegram}</a>\n"));
                        await _client.SendTextMessageAsync(-1001407833026, text, ParseMode.Html, cancellationToken: ct);
                        break;
                    }
            }
        }
    }
}
