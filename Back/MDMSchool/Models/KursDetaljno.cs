using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Models
{
    public class KursDetaljno
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String Id { get; set; }
        public int Cena { get; set; }
        public String DuziOpis { get; set; }

        //mozda je logicnije da u kursuDetaljno stoji referenca na kursOsnovno zato sto ta podela treba da smanji broj podataka prilikom pretrage 
        //a kad se pretrazuju kursevi vrate se i kursDetaljno informacije kroz referencu

        //ja bih rekla da grupe treba da imaju termine a ne ovde da stoje, i da se prilikom rezervacije bira grupa za koju korisnik zeli
        //da rezervise mesto (znaci da dodamo referencu na grupu u rezervaciji i onda se tacno odredjenoj grupi povecava ili smanjuje broj polaznika)
        //na frontu onda da kad otvorimo kurs mozemo da pregledamo sve grupe i pored svake da stoji mogucnost rezervacije
        //onda u sustini rezervacija nije vezana za kurs nego za grupu (sto i daje vise smisla postojanju grupe kao klase)
        public List<String> Termini { get; set; }
        
        //[JsonIgnore]
        //[BsonIgnore]
        //public KursOsnovno OsnovnoKurs { get; set; }
    }
}