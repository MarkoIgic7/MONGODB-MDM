using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Models
{
    [BsonIgnoreExtraElements]
    public class Kategorija
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        public String Uzrast { get; set; }

        public List<KursOsnovno> Kursevi { get; set; }
        public Skola Skola { get; set; }

    }
}