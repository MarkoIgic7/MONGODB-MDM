using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Models
{
    [BsonIgnoreExtraElements]
    public class KursOsnovno
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        public String Naziv { get; set; }
        public String Jezik { get; set; }
        public int Cena { get; set; }
        public String DuziOpis { get; set; }

        public String KratakOpis { get; set; }

        public List<Spoj> Spojevi { get; set; }

        [JsonIgnore]
        public Kategorija Kategorija { get; set; }

   
    
    }
}