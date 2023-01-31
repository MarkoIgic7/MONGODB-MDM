using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class KursOsnovno
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        public String Naziv { get; set; }
        public String Jezik { get; set; }

        public String KratakOpis { get; set; }

        public Profesor  Profesor { get; set; }
        public Kategorija Kategorija { get; set; }
        public KursDetaljno DetaljnijeKurs { get; set; }
        public List<MongoDBRef> Rezervacije { get; set; }
        public List<Grupa> Grupe { get; set; }
        
    }
}