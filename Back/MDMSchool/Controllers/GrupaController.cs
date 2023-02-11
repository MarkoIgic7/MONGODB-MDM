using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupaController : ControllerBase
    {
        private IMongoCollection<Grupa> GrupaCollection;
        private IMongoCollection<Spoj> SpojCollection;

        private IMongoCollection<KursOsnovno> KursCollection;
        private IMongoCollection<Profesor> ProfesorCollection;
        
        public GrupaController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Grupa>("Grupa");
            GrupaCollection = collection;
            var collection2 = db.GetCollection<Spoj>("Spoj");
            SpojCollection = collection2;
            var collection3 = db.GetCollection<KursOsnovno>("Kurs");
            KursCollection = collection3;
            var collection4 = db.GetCollection<Profesor>("Profesor");
            ProfesorCollection = collection4;
           
        }

        [HttpPost]
        [Route("DodajGrupu/{naziv}/{tbr}/{maxbr}/{idKursa}/{idProf}/{termini}")]
        public async Task<ActionResult> DodajGrupu(String naziv, String tbr, String maxbr, String idKursa, String idProf, String termini)
        {
            int TBR = int.Parse(tbr);
            int MAXBR = int.Parse(maxbr);
            var k = KursCollection.Find(k => k.Id == idKursa).FirstOrDefault();
            var p = ProfesorCollection.Find(k => k.Id == idProf).FirstOrDefault();
            Spoj s=new Spoj();
            s.Profesor=p;
            s.OsnovnoKurs=k;
            s.Grupe=new List<Grupa>();
            SpojCollection.InsertOne(s);
            
            p.Spojevi.Add(s);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(p, jsonSerializerSettings);
            var filter = Builders<Profesor>.Filter.Eq("Id",idProf);
            var profesorObject = JsonConvert.DeserializeObject<Profesor>(json);
            await ProfesorCollection.ReplaceOneAsync(filter, profesorObject, new ReplaceOptions { IsUpsert = true });

            k.Spojevi.Add(s);

            var jsonSerializerSettings1 = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json1 = JsonConvert.SerializeObject(k, jsonSerializerSettings1);
            var filter1 = Builders<KursOsnovno>.Filter.Eq("Id",idKursa);
            var kursObject = JsonConvert.DeserializeObject<KursOsnovno>(json1);
            await KursCollection.ReplaceOneAsync(filter1, kursObject, new ReplaceOptions { IsUpsert = true });
            //await KursCollection.ReplaceOneAsync(k1=>k1.Id==idKursa,k);
            //await ProfesorCollection.ReplaceOneAsync(p1=>p1.Id==idProf,p);

            Grupa g = new Grupa();
            g.MaximalniBroj = MAXBR;
            g.TrenutniBroj = TBR;
            g.Naziv = naziv;
            g.Spoj=s;
            g.Termini=new List<String>();
            string[] arr=termini.Split('*');
            foreach( string a in arr)
            {
                g.Termini.Add(a);
            }
            GrupaCollection.InsertOne(g);

            s.Grupe.Add(g);

            var jsonSerializerSettings2 = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json2 = JsonConvert.SerializeObject(s, jsonSerializerSettings2);
            var filter2 = Builders<Spoj>.Filter.Eq("Id",s.Id);
            var spojObject = JsonConvert.DeserializeObject<Spoj>(json2);
            await SpojCollection.ReplaceOneAsync(filter2, spojObject, new ReplaceOptions { IsUpsert = true });
            //await SpojCollection.ReplaceOneAsync(p1=>p1.Id==s.Id, s);

            //k.Grupe.Add(g);
            return Ok(g);

        }

        [HttpGet]
        [Route("PreuzmiSveGrupe/{idKursa}")]
        public async Task<ActionResult> PreuzmiSveGrupe(String idKursa)
        {
            var spojevi=SpojCollection.Find(s=>s.OsnovnoKurs.Id==idKursa).ToList();
            var lista=new List<object>();
         
            foreach(var s in spojevi)
            {
                foreach(Grupa g in s.Grupe)
                {
                   lista.Add(new{ 
                    ProfesorIme=s.Profesor.Ime,
                    ProfesorPrezime=s.Profesor.Prezime,
                    ProfesorPodaci=s.Profesor.Podaci,
                    ProfesorStrucnaSprema=s.Profesor.StrucnaSprema,
                    GrupaId=g.Id,
                    Termini=g.Termini,
                    TrenutniBroj = g.TrenutniBroj,
                    MaximalniBroj = g.MaximalniBroj,
                    Naziv = g.Naziv
                });
                }
            }
            return Ok(lista);

        }
        [HttpPut]
        [Route("IzmeniGrupu/{idGrupe}/{naziv}/{trBr}/{maxBr}/{termini}")]
        public async Task<ActionResult> IzmeniGrupu(String idGrupe,String naziv,String trBr,String maxBr,String termini)
        {
            int trenutniBroj = int.Parse(trBr);
            int maximalniBroj = int.Parse(maxBr);


            var grupa = GrupaCollection.Find(g => g.Id==idGrupe).FirstOrDefault();
            grupa.Naziv = naziv;
            grupa.TrenutniBroj = trenutniBroj;
            grupa.MaximalniBroj = maximalniBroj;

            
            grupa.Termini.Clear();
            string[] niz = termini.Split("*");
            foreach(string n in niz)
            {
                grupa.Termini.Add(n);
            }

            var filter = Builders<Grupa>.Filter.Eq(g => g.Id, idGrupe);
            await GrupaCollection.ReplaceOneAsync(filter, grupa);


            //mora da se izmeni grupa i unutar svih spojeva gde se nalazi jer se VratiSveGrupe() vadi iz Spojeva
            // Unutar Spoja ostane stara Grupa (neizmenjena)
            
            var spojevi = SpojCollection.Find(_ => true).ToList();
            int index;
            foreach(var s in spojevi)
            {
                foreach(Grupa g1 in s.Grupe)
                {
                    if(g1.Id==idGrupe)
                    {
                        //to je ta grupa koja treba da se izmeni
                        g1.Naziv = naziv;
                        g1.MaximalniBroj = maximalniBroj;
                        g1.TrenutniBroj = trenutniBroj;

                        g1.Termini.Clear();
                        string[] niz1 = termini.Split("*");
                        foreach(string n in niz1)
                        {
                            g1.Termini.Add(n);
                        }

                        

                    }
                }
                var filterSpoj = Builders<Spoj>.Filter.Eq(s1 => s1.Id,s.Id);
                await SpojCollection.ReplaceOneAsync(filterSpoj,s);

            }
            
            return Ok(new{
                Id = grupa.Id,
                Naziv = grupa.Naziv,
                TrenutniBroj = grupa.TrenutniBroj,
                MaximalniBroj = grupa.MaximalniBroj,
                Termini = grupa.Termini
            });
        }
    }
}