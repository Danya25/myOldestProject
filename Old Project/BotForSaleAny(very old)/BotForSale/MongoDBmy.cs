using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BotForSale
{
    class MainWork
    {
        public async Task<List<string>> GetInformCS(string name)
        {
            List<string> peoples = new List<string>();
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var collection = database.GetCollection<Acc>("Acc");

            var filter = new BsonDocument("NameVPN", name);
            var people = await collection.Find(filter).Project<Acc>("{NameVPN:1,Time:1,Cost:1,_id:1}").ToListAsync();

            var AccsGames = new List<string>();

            foreach (var p in people)
            {
                AccsGames.Add($"ID: {p.Id} - Название: {p.NameVPN} - Цена: {p.Cost} - Работает до: {p.Time}");
            }
            return AccsGames;
        }

        public async Task<List<string>> BuyId(string[] id)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var purchasedAccs = database.GetCollection<Acc>("PurchasedAcc");
            var collection = database.GetCollection<Acc>("Acc");
            List<ObjectId> all_id = new List<ObjectId>();
            foreach (string word in id)
            {
               var word_temp = ObjectId.Parse(word);
               all_id.Add(word_temp);
            }
            var buyAccs = new List<string>();
            foreach (var word in all_id)
            {
                var filter = new BsonDocument("_id", word);
                //var accs = await collection.Find(filter).Project<Acc_OutPut>("{Name:1,Url:1,Login:1,Pass:1}").ToListAsync();
                var accs = await collection.Find(acc => acc.Id == word).Project(acc => new{acc.NameVPN, acc.Mail, acc.Password, acc.Time }).ToListAsync();
                buyAccs.AddRange(accs.Select(acc => acc.ToString()));
                var items = await collection.Find(new BsonDocument("_id", word)).ToListAsync();
                await purchasedAccs.InsertManyAsync(items);
                collection.DeleteOne(filter);
            }
            return buyAccs;
        }
        public async Task<int> CostsID(string[] id)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var collection = database.GetCollection<Acc>("Acc");

            List<ObjectId> all_id = new List<ObjectId>();

            foreach (string word in id)
            {
                var word_temp = ObjectId.Parse(word);
                all_id.Add(word_temp);
            }

            List<int> cost = new List<int>();
            foreach (var word in all_id)
            {
               cost.AddRange(await collection.Find(acc => acc.Id == word).Project(acc => acc.Cost).ToListAsync());
            }
            int sum = cost.Sum();
            return sum;
        }

        public async Task<List<bool>> CheckInDBAcc(string[] id)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var collection = database.GetCollection<Acc>("Acc");
            List<bool> AccInBD = new List<bool>();
            foreach(var word in id)
            {
                var idObj = ObjectId.Parse(word);
                var filter = new BsonDocument("_id", idObj);
                var found = await collection.Find(filter).AnyAsync();
                AccInBD.Add(found);
            }
            return AccInBD;
            
        }
    }

    //[BsonIgnoreExtraElements]
    class Acc
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string NameVPN { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Time { get; set; }
        public int Cost { get; set; }
    }
    [BsonIgnoreExtraElements]
    class Acc_OutPut
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
    }
}
