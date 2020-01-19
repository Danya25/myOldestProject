using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zakaz25WebApi.Models;

namespace Zakaz25WebApi.Context
{
    public class DataContext
    {
        public readonly IMongoCollection<User> _User;
        public DataContext(IOptions<DataOptions> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);

            _User = database.GetCollection<User>(nameof(User));
        }
    }
}
