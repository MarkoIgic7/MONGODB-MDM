using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Notifikacija
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public String Naziv { get; set; }
    }
}