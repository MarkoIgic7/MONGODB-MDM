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
            var coll4 = db.GetCollection<Grupa>("Grupa");
            GrupaCollection = coll4;
            
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
            var grupe = GrupaCollection.Find(g=>g.OsnovnoKurs==kurs).ToList();
            foreach(var g in grupe)
            {
                if(g.TrenutniBroj<g.MaximalniBroj)
                    {
                        imaMesta = true;
                        g.TrenutniBroj=g.TrenutniBroj+1;
                        var filter = Builders<Grupa>.Filter.Eq("Id",g.Id);
                        var update = Builders<Grupa>.Update.Set("TrenutniBroj",g.TrenutniBroj);
                        GrupaCollection.UpdateOne(filter,update);
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

                //var korisnik=UserCollection.FindOne(Builders<User>.Filter.Eq("_id", mongoDbRef.Id));

                var korisnik = await UserCollection.Find(a => a.Id == r.Korisnik.Id.ToString()).FirstOrDefaultAsync();
                var kurs = await KursCollection.Find(a => a.Id == r.OsnovniKurs.Id.ToString()).FirstOrDefaultAsync();

                lista.Add(new{
                    Id=r.Id,
                    Status  = r.Status,
                    Korisnik=korisnik.Mail,
                    Kurs=kurs.Naziv
                });

                 //var kursFilter = Builders<KursOsnovno>.Filter.Eq(a => a.Id, idKurs);
            //var kurs = KursCollection.Find(kursFilter).FirstOrDefault();
            
            //var userFilter = Builders<User>.Filter.Eq(b => b.Id, idKorisnik);
            //var user   = UserCollection.Find(userFilter).FirstOrDefault();
                //lista.Add(new{})
            }
            
            
            return Ok(lista);
        }

        [HttpDelete]
        [Route("ObrisiRezervaciju/{idRezervacije}")]
        public async Task<ActionResult> ObrisiRezervaciju(String idRezervacije)
        {
            //OVDE TREBA DA SE SMANJUJE TRENUTNI BROJ NEKOJ GRUPI 
            var filter = Builders<Rezervacija>.Filter.Eq("Id",idRezervacije);
            await RezervacijaCollection.DeleteOneAsync(filter);
            return Ok("Obrisana rezervacija");
        }
    }
}