using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class User
    {
        public ObjectId _id { get; set; }
        public String Mail  { get; set; }
        public String  Password { get; set; }
            

        public List<MongoDBRef> Rezervacije { get; set; }
    }
}