using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zakaz25WebApi.Models
{
    public class Img
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Url { get; set; }
    }
}
