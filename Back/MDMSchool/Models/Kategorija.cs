using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class Kategorija
    {
        public ObjectId _id { get; set; }

        public String Uzrast { get; set; }
        public List<MongoDBRef> Kursevi { get; set; }
        public MongoDBRef Skola { get; set; }
    }
}