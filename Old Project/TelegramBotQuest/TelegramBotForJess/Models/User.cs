using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TelegramBotForJess.Models
{
    internal sealed class User
    {
        [BsonId]
        public int IdTelegram { get; set; }
        [BsonIgnoreIfDefault]
        public bool IsAnswer { get; set; }
        public string Status { get; set; }
        [BsonIgnoreIfNull]
        public List<string> Answers { get; set; }
    }
}
