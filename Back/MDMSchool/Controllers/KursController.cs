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
        }

        [HttpPost]
        [Route("DodajKurs/{naziv}/{jezik}/{kratakOpis}/{idKat}/{cena}/{duziOpis}")]
        public async Task<ActionResult> DodajKurs(String naziv, string jezik, string kratakOpis, string idKat, string cena, string duziOpis)
        {
            int c = int.Parse(cena);

            KursOsnovno k = new KursOsnovno();
            k.Naziv = naziv;
            k.Jezik=jezik;
            k.KratakOpis=kratakOpis;
            k.Spojevi=new List<Spoj>();
            
            var kategorija=KategorijaCollection.Find(p=>p.Id==idKat).FirstOrDefault();
            
            k.Kategorija=kategorija;
            k.Cena=c;
            k.DuziOpis=duziOpis;
    
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
            //da probamo ovde sa kategorija umesto kategorijaObject
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
                var kursevi=KursCollection.Find(p=>p.Kategorija.Id==idKat).ToList();
                return Ok(kursevi);
            }
            else{
                var kursevi=KursCollection.Find(p=>p.Kategorija.Id==idKat && p.Jezik==jezik).ToList();
                return Ok(kursevi);

            }
        }

        [HttpGet]
        [Route("PreuzmiCeoKurs/{idKursa}")]
        public async Task<ActionResult> PreuzmiCeoKurs(string idKursa)
        {

            //var kursOsnovno=KursCollection.Find(p=>p.Id==idKursa).FirstOrDefault();

            //ako bi se vratilo samo ovo(kursOsnovno odozgo) ne bi vratio kategoriju zbog JsonIgnore-a
            //zbog toga sam stavio anonomni tip

            //MOZDA DA PROBAMO DA STAVIMO NA DRUGOM MESTU JSONIGNORE?
            var kurs = await KursCollection.Find(k => k.Id == idKursa).FirstOrDefaultAsync();

            return Ok(new{
                Id = kurs.Id,
                Naziv = kurs.Naziv,
                Cena = kurs.Cena,
                DuziOpis = kurs.DuziOpis,
                KratakOpis = kurs.KratakOpis,
                IdKategorije = kurs.Kategorija.Id,
                Uzrast = kurs.Kategorija.Uzrast,
                Jezik = kurs.Jezik
            });

        }

        [HttpPut]
        [Route("IzmeniKurs/{idKursa}/{naziv}/{jezik}/{kraciOpis}/{cena}/{duziOpis}")]
        public async Task<ActionResult> IzmeniKurs(string idKursa, string naziv, string jezik, string kraciOpis, string cena, string duziOpis)
        {

            var kurs = await KursCollection.Find(k => k.Id == idKursa).FirstOrDefaultAsync();
            kurs.Naziv=naziv;
            kurs.Jezik=jezik;
            kurs.KratakOpis=kraciOpis;
            kurs.Cena=int.Parse(cena);
            kurs.DuziOpis=duziOpis;

            var filter = Builders<KursOsnovno>.Filter.Eq(x => x.Id, idKursa);
            await KursCollection.ReplaceOneAsync(filter, kurs);
            
            return Ok(kurs);

        }

        
    }
}