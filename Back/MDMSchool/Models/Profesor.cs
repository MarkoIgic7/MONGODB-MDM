using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Profesor

    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String Podaci { get; set; }
        public String StrucnaSprema { get; set; }

       // public Skola Skola { get; set; }

        //public List<KursOsnovno> Kursevi { get; set; }
        public List<Spoj> Spojevi { get; set; }
    }
}