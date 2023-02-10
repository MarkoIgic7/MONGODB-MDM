using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KursController : ControllerBase
    {
        private IMongoCollection<KursOsnovno> KursCollection;
        private IMongoCollection<KursDetaljno> KursDetaljnoCollection;
         private IMongoCollection<Kategorija> KategorijaCollection;
         private IMongoCollection<Profesor> ProfesorCollection;
        //private readonly FilterDefinitionBuilder<KursOsnovno> filterBuilder = Builders<KursOsnovno>.Filter;
        public KursController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<KursOsnovno>("Kurs");
            KursCollection = collection;
            var collection1 = db.GetCollection<Kategorija>("Kategorija");
            KategorijaCollection = collection1;
            var collection2 = db.GetCollection<Profesor>("Profesor");
            ProfesorCollection = collection2;
            var collection3 = db.GetCollection<KursDetaljno>("KursDetaljno");
            KursDetaljnoCollection = collection3;
        }

        [HttpPost]
        [Route("DodajKurs/{naziv}/{jezik}/{kratakOpis}/{idKat}/{idProf}/{cena}/{duziOpis}/{termini}")]
        public async Task<ActionResult> DodajKurs(String naziv, string jezik, string kratakOpis, string idKat, string idProf, string cena, string duziOpis, string termini)
        {
            int c = int.Parse(cena);

            KursOsnovno k = new KursOsnovno();
            k.Naziv = naziv;
            k.Jezik=jezik;
            k.KratakOpis=kratakOpis;
            
            var kategorija=KategorijaCollection.Find(p=>p.Id==idKat).FirstOrDefault();
            var profesor=ProfesorCollection.Find(p=>p.Id==idProf).FirstOrDefault();
            
            k.Kategorija=kategorija;
            k.Profesor=profesor;

            KursDetaljno kd=new KursDetaljno();
            kd.Cena=c;
            kd.DuziOpis=duziOpis;
            kd.Termini=new List<String>();
            string[] arr=termini.Split('#');
            foreach( string a in arr)
            {
                kd.Termini.Add(a);
            }
            //kd.OsnovnoKurs=k;
            KursDetaljnoCollection.InsertOne(kd);

            k.DetaljnijeKurs=kd;
            await KursCollection.InsertOneAsync(k);


            kategorija.Kursevi.Add(k);

            var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            var json = JsonConvert.SerializeObject(kategorija, jsonSerializerSettings);
            var filter = Builders<Kategorija>.Filter.Eq("Id",idKat);
            var kategorijaObject = JsonConvert.DeserializeObject<Kategorija>(json);
            await KategorijaCollection.ReplaceOneAsync(filter, kategorijaObject, new ReplaceOptions { IsUpsert = true });

            profesor.Kursevi.Add(k);
             var jsonSerializerSettings1 = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
             
            var json1 = JsonConvert.SerializeObject(profesor, jsonSerializerSettings1);
            var filter1 = Builders<Profesor>.Filter.Eq("Id",idProf);
            var profesorObject = JsonConvert.DeserializeObject<Profesor>(json1);
            await ProfesorCollection.ReplaceOneAsync(filter1, profesorObject, new ReplaceOptions { IsUpsert = true });
            
            return Ok(k);

        }
        
        [HttpGet]
        [Route("PreuzmiSveJezike")]
        public async Task<ActionResult> PreuzmiSveJezike()
        {
             var kursevi =  KursCollection.Find(_=>true).ToList();
             List<String> jezici=new List<String>();
             foreach(var k in kursevi)
             {
                jezici.Add(k.Jezik);
             }
             return Ok(jezici.Distinct());

        }

        [HttpGet]
        [Route("PreuzmiSveKurseve/{idKat}/{jezik}")]
        public async Task<ActionResult> PreuzmiSveKurseve(string idKat, string jezik)
        {
            if(idKat=="nema")
            {
                return BadRequest("Morate uneti kategoriju!");
            }
            else if(jezik=="nema")
            {
                var kat=KategorijaCollection.Find(p=>p.Id==idKat).FirstOrDefault();
                var kursevi=KursCollection.Find(p=>p.Kategorija==kat).ToList();
                return Ok(kursevi);
            }
            else{
                var kat=KategorijaCollection.Find(p=>p.Id==idKat).FirstOrDefault();
                var kursevi=KursCollection.Find(p=>p.Kategorija==kat && p.Jezik==jezik).ToList();
                return Ok(kursevi);

            }
        }

        [HttpGet]
        [Route("PreuzmiCeoKurs/{idKursa}")]
        public async Task<ActionResult> PreuzmiCeoKurs(string idKursa)
        {

            //var kursOsnovno=KursCollection.Find(p=>p.Kurs.Id==idKursa).FirstOrDefault();

            return Ok();

        }

        
    }
}