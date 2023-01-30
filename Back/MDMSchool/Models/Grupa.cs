using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class Grupa
    {
        public ObjectId _id { get; set; }
        public String Naziv { get; set; }
        public int TrenutniBroj { get; set; }
        public int MaximalniBroj { get; set; }

        public MongoDBRef OsnovnoKurs { get; set; }
        
    }
}