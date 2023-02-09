using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;


namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RezervacijaController : ControllerBase
    {
        private IMongoCollection<Rezervacija> RezervacijaCollection;
        private IMongoCollection<User> UserCollection;
        private IMongoCollection<KursOsnovno> KursCollection;

        private IMongoCollection<Grupa> GrupaCollection;

        private IMongoDatabase db;
        
        public RezervacijaController(IMongoClient mc)
        {
            db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Rezervacija>("Rezervacija");
            RezervacijaCollection = collection;
            var col2 = db.GetCollection<User>("Korisnici");
            UserCollection = col2;
            var coll3 = db.GetCollection<KursOsnovno>("Kurs");
            KursCollection = coll3;
            
        }

        [HttpPost]
        [Route("DodajRezervaciju/{idKurs}/{idKorisnik}")]
        public async Task<ActionResult> DodajRezervaciju(String idKurs,String idKorisnik)
        {

            var kursFilter = Builders<KursOsnovno>.Filter.Eq(a => a.Id, idKurs);
            var kurs = KursCollection.Find(kursFilter).FirstOrDefault();
            
            var userFilter = Builders<User>.Filter.Eq(b => b.Id, idKorisnik);
            var user   = UserCollection.Find(userFilter).FirstOrDefault();

            bool imaMesta = false;
            foreach(Grupa g in kurs.Grupe)
            {
                if(g.TrenutniBroj<g.MaximalniBroj)
                    {
                        imaMesta = true;
                        break;
                    }
            }
            if(imaMesta)
            {
                
            Rezervacija r = new Rezervacija();
            r.Korisnik = new MongoDBRef("Korisnik",user.Id);
            r.OsnovniKurs = new MongoDBRef("OsnovniKurs", kurs.Id);
            r.VremeRezervacije = DateTime.Now;
            r.Status = true;

            r.OsnovniKurs = new MongoDBRef("Kurs",kurs.Id);
            r.Korisnik = new MongoDBRef("Korisnik", user.Id);

            RezervacijaCollection.InsertOne(r);


            var update = Builders<User>.Update.AddToSet(b => b.Rezervacije, new MongoDBRef("Rezervacija", r.Id));
            UserCollection.UpdateOne(userFilter, update);

            
            var update1 = Builders<KursOsnovno>.Update.AddToSet(b => b.Rezervacije, new MongoDBRef("Rezervacija", r.Id));
            KursCollection.UpdateOne(kursFilter, update1);
            
            
            return Ok("Dodata rezervacija");
            }
            else
            {
                return BadRequest("Nazalost, sve grupe su pune ");
            }

        }
        [HttpGet]
        [Route("PreuzmiSveRezervacije")]
        public async Task<ActionResult> PreuzmiSveRezervacije()
        {
            var rezervacije = RezervacijaCollection.Find(_ => true).ToList();
            var lista = new List<object>();
            //var resolver = new MongoDBRefResolver(db);

            foreach(var r in rezervacije)
            {
                if(r.VremeRezervacije.AddDays(2)<DateTime.Now)
                {
                    r.Status = false;
                    var filter = Builders<Rezervacija>.Filter.Eq("Id",r.Id);
                    var update = Builders<Rezervacija>.Update.Set("Status",r.Status);
                    RezervacijaCollection.UpdateOne(filter,update);

                }
                 //var kursFilter = Builders<KursOsnovno>.Filter.Eq(a => a.Id, idKurs);
            //var kurs = KursCollection.Find(kursFilter).FirstOrDefault();
            
            //var userFilter = Builders<User>.Filter.Eq(b => b.Id, idKorisnik);
            //var user   = UserCollection.Find(userFilter).FirstOrDefault();
                //lista.Add(new{})
            }
            
            
            return Ok(rezervacije.Select( r => 
            new{
                Id = r.Id,
                Status  = r.Status,
                
            }));
        }
    }
}