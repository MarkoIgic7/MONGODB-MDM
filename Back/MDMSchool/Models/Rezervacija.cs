using MongoDB.Bson;
using MongoDB.Driver;

namespace Models
{
    public class Rezervacija
    {
        public ObjectId _id { get; set; }

        public MongoDBRef Korisnik { get; set; }
        public MongoDBRef OsnovniKurs { get; set; }
    }
}