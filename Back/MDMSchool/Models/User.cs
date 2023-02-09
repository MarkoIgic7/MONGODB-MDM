using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public String Mail  { get; set; }
        public String  Password { get; set; }
        
        public List<MongoDBRef> Rezervacije { get; set; }

        public User()
        {
            Rezervacije = new List<MongoDBRef>();
        }
    }
}