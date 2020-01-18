using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zakaz25WebApi.Models
{
    public class User
    {
        [BsonId]
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
