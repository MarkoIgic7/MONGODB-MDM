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
        private IMongoCollection<Grupa> GrupaCollection;

        private IMongoDatabase db;
        
        public RezervacijaController(IMongoClient mc)
        {
            db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Rezervacija>("Rezervacija");
            RezervacijaCollection = collection;
            var col2 = db.GetCollection<User>("Korisnici");
            UserCollection = col2;
            var coll4 = db.GetCollection<Grupa>("Grupa");
            GrupaCollection = coll4;
            
        }

        [HttpPost]
        [Route("DodajRezervaciju/{idGrupe}/{idKorisnik}")]
        public async Task<ActionResult> DodajRezervaciju(String idGrupe,String idKorisnik)
        {

            var grupaFilter = Builders<Grupa>.Filter.Eq(a => a.Id, idGrupe);
            var grupa = GrupaCollection.Find(grupaFilter).FirstOrDefault();
            
            var userFilter = Builders<User>.Filter.Eq(b => b.Id, idKorisnik);
            var user   = UserCollection.Find(userFilter).FirstOrDefault();

            var rezervacije = RezervacijaCollection.Find(_=>true).ToList();
            Boolean postoji = false;
        
            if(grupa.TrenutniBroj<grupa.MaximalniBroj)
            {
                    //ovde da se doda provera da li je taj korisnik vec napravio rezervaciju za istu grupu
                    foreach(var r1 in rezervacije)
                    {
                        var user1 = await UserCollection.Find(u =>u.Id==r1.Korisnik.Id.ToString()).FirstOrDefaultAsync();
                        var grupa1 = await GrupaCollection.Find(g => g.Id == r1.Grupa.Id.ToString()).FirstOrDefaultAsync();
                        if(user1!=null && grupa1!=null)
                        {
                            postoji = true;
                            break;
                        }
                    }
                    if(postoji==false)
                    {
                        grupa.TrenutniBroj=grupa.TrenutniBroj+1;
                        var filter = Builders<Grupa>.Filter.Eq("Id",grupa.Id);
                        var update = Builders<Grupa>.Update.Set("TrenutniBroj",grupa.TrenutniBroj);
                        GrupaCollection.UpdateOne(filter,update);

                        Rezervacija r = new Rezervacija();
                        r.Korisnik = new MongoDBRef("Korisnik",user.Id);
                        r.Grupa = new MongoDBRef("Grupa", grupa.Id);
                        r.VremeRezervacije = DateTime.Now;
                        r.Status = true;

                        RezervacijaCollection.InsertOne(r);


                        var update2 = Builders<User>.Update.AddToSet(b => b.Rezervacije, new MongoDBRef("Rezervacija", r.Id));
                        UserCollection.UpdateOne(userFilter, update2);

                        
                        var update1 = Builders<Grupa>.Update.AddToSet(b => b.Rezervacije, new MongoDBRef("Rezervacija", r.Id));
                        GrupaCollection.UpdateOne(grupaFilter, update1);
                        
                        
                        return Ok("Dodata rezervacija");
            

                    }
                    else
                    {
                        return BadRequest("Korisnik je vec uputio rezervaciju za ovu grupu");
                    }
                    
                
            }
            else
            {
                return BadRequest("Nazalost, u ovoj grupi nema mesta ");
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
                var grupa = await GrupaCollection.Find(a => a.Id == r.Grupa.Id.ToString()).FirstOrDefaultAsync();

                lista.Add(new{
                    Id=r.Id,
                    Status  = r.Status,
                    Korisnik=korisnik.Mail,
                    Grupa=grupa.Naziv
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
            //na frontu nek stavi da mogu da se obrisu samo rezervacije sa statusom false i onda ovde nema provere
            var r=RezervacijaCollection.Find(r1=>r1.Id==idRezervacije).FirstOrDefault();
            var grupa = await GrupaCollection.Find(a => a.Id == r.Grupa.Id.ToString()).FirstOrDefaultAsync();

            grupa.TrenutniBroj--;
            var filter = Builders<Grupa>.Filter.Eq("Id",grupa.Id);
            var update = Builders<Grupa>.Update.Set("TrenutniBroj",grupa.TrenutniBroj);
            GrupaCollection.UpdateOne(filter,update);


            var filter1 = Builders<Rezervacija>.Filter.Eq("Id",idRezervacije);
            await RezervacijaCollection.DeleteOneAsync(filter1);
            return Ok("Obrisana rezervacija");
        }
    }
}