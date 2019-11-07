using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TelegramBotForJess.Models;
using TelegramBotForJess.Options;

namespace TelegramBotForJess.Data
{
    internal sealed class DataContext
    {
        public readonly IMongoCollection<User> UserColl;

        public DataContext(IOptions<DataOptions> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var dataBase = mongoClient.GetDatabase(options.Value.DatabaseName);

            UserColl = dataBase.GetCollection<User>(nameof(User));

        }
    }
}
