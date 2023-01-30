using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class KursOsnovno
    {
        public ObjectId _id { get; set; }

        public String Naziv { get; set; }
        public String Jezik { get; set; }

        public String KratakOpis { get; set; }

        public MongoDBRef  Profesor { get; set; }
        public MongoDBRef Kategorija { get; set; }
        public MongoDBRef DetaljnijeKurs { get; set; }
        public List<MongoDBRef> Rezervacije { get; set; }
        public List<MongoDBRef> Grupe { get; set; }
        
    }
}