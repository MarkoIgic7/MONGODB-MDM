using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Models
{
    public class KursDetaljno
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public int Cena { get; set; }
        public String DuziOpis { get; set; }
        public List<String> Termini { get; set; }
        
        [JsonIgnore]
        [BsonIgnore]
        public KursOsnovno OsnovnoKurs { get; set; }
    }
}