using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class Skola

    {
        public ObjectId _id { get; set; }
        public String Naziv { get; set; }
        public String Lokacija { get; set; }
        public String Kontakt { get; set; }
        public String Opis { get; set; }

        public List<MongoDBRef> Kategorije { get; set; }
        public List<MongoDBRef> Profesori { get; set; }
        //public List<MongoDBRef> Kursevi { get; set; }
        
    }
}