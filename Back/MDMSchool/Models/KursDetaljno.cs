using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class KursDetaljno
    {
        public ObjectId _id { get; set; }
        public int Cena { get; set; }
        public String DuziOpis { get; set; }
        public List<String> Termini { get; set; }

        public MongoDBRef OsnovnoKurs { get; set; }
    }
}