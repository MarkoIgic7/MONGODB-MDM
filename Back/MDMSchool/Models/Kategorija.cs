using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Kategorija
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        public String Uzrast { get; set; }
        public List<MongoDBRef> Kursevi { get; set; }
        public MongoDBRef Skola { get; set; }
    }
}