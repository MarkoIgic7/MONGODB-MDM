using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Models
{
    public class Grupa
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public String Naziv { get; set; }
        public int TrenutniBroj { get; set; }
        public int MaximalniBroj { get; set; }

        //mozda nam treba klasa Spoj za profesora i kursOsnovno jer je m:n veza?
        [JsonIgnore]
        [BsonIgnore]
        public Spoj Spoj { get; set; }
        public List<MongoDBRef> Rezervacije { get; set; }
        
        public List<String> Termini { get; set; }

        public Grupa()
        {
            Rezervacije = new List<MongoDBRef>();
        }
    }
}