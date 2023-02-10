using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class Spoj
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        [JsonIgnore]
        public Profesor Profesor { get; set; }

        [JsonIgnore]
        public KursOsnovno OsnovnoKurs { get; set; }

        public List<Grupa> Grupe { get; set; }

        
    }
}