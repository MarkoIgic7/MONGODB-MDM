using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models
{
    public class Rezervacija
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }

        public MongoDBRef Korisnik { get; set; }
        public MongoDBRef OsnovniKurs { get; set; }
    }
}