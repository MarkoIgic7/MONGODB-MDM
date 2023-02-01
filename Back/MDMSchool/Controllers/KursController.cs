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
            kd.OsnovnoKurs=k;
            KursDetaljnoCollection.InsertOne(kd);

            k.DetaljnijeKurs=kd;
            KursCollection.InsertOne(k);

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

        
    }
}