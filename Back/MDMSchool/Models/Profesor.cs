using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class Profesor

    {
        public ObjectId _id { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String Podaci { get; set; }
        public String StrucnaSprema { get; set; }

        public MongoDBRef Skola { get; set; }

        public List<MongoDBRef> Kursevi { get; set; }
    }
}