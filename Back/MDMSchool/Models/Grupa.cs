using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

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
        public KursOsnovno OsnovnoKurs { get; set; }
        
    }
}