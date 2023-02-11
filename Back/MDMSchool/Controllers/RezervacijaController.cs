using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RezervacijaController : ControllerBase
    {
        private IMongoCollection<Rezervacija> RezervacijaCollection;
        private IMongoCollection<User> UserCollection;
        private IMongoCollection<Grupa> GrupaCollection;
        private IMongoCollection<Spoj> SpojCollection;

        private IMongoCollection<Notifikacija> NotifikacijaCollection;

        private IMongoDatabase db;

        public IHubContext<Notif,INotifHub> NotifHub {get;set;}
        
        public RezervacijaController(IMongoClient mc, IHubContext<Notif,INotifHub> hub)
        {
            db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Rezervacija>("Rezervacija");
            RezervacijaCollection = collection;
            var col2 = db.GetCollection<User>("Korisnici");
            UserCollection = col2;
            var coll4 = db.GetCollection<Grupa>("Grupa");
            GrupaCollection = coll4;
            var coll5 = db.GetCollection<Spoj>("Spoj");
            SpojCollection = coll5;
            var coll6 = db.GetCollection<Notifikacija>("Notifikacije");
            NotifikacijaCollection = coll6;

            NotifHub = hub;
            
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
            
            if(lista.Count()==0)
            {
                return BadRequest("Ne postoje rezervacije");
            }
            else
            {
                return Ok(lista);
            }
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


            // deo za notifikacije
            //grupu imamo 159 linija
            var korisnik = await UserCollection.Find(a => a.Id == r.Korisnik.Id.ToString()).FirstOrDefaultAsync();

            //nalazenje kursa
            var spojevi = SpojCollection.Find(_ => true).ToList();
            string nazivKursa = "";
            foreach(var s in spojevi)
            {
                foreach(Grupa g in s.Grupe)
                {
                    if(g.Id==grupa.Id)
                    {
                        nazivKursa = s.OsnovnoKurs.Naziv;
                        break;
                    }
                }
            }
            //Console.WriteLine(spoj);
            Console.WriteLine("Korisnik :" + korisnik.Mail);
            Console.WriteLine("Grupa : " + grupa.Naziv);
            Console.WriteLine("Kurs : "+ nazivKursa);
            
            
            Notifikacija n = new Notifikacija();
            n.Naziv = "Vasa rezervacija za kurs : "+nazivKursa+" i grupu : "+ grupa.Naziv+" je istekla !";
            await NotifikacijaCollection.InsertOneAsync(n);

            korisnik.Notifikacije.Add(n);

            var filterUser = Builders<User>.Filter.Eq("Id",korisnik.Id);
            var updateUser = Builders<User>.Update.Set("Notifikacije",korisnik.Notifikacije);
            UserCollection.UpdateOne(filterUser,updateUser);

            await NotifHub.Clients.Group(korisnik.Id).SendMessageToAll(n.Naziv,korisnik.Id);

            return Ok("Obrisana rezervacija");
        }
    }
}