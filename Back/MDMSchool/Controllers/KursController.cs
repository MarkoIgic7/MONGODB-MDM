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
         private IMongoCollection<Kategorija> KategorijaCollection;
        //private readonly FilterDefinitionBuilder<KursOsnovno> filterBuilder = Builders<KursOsnovno>.Filter;
        public KursController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<KursOsnovno>("Kurs");
            KursCollection = collection;
            var collection1 = db.GetCollection<Kategorija>("Kategorija");
            KategorijaCollection = collection1;
        }

        [HttpPost]
        [Route("DodajKurs/{naziv}/{jezik}/{kratakOpis}/{idKat}/{idProf}")]
        public async Task<ActionResult> DodajKurs(String naziv, string jezik, string kratakOpis, string idKat, string idProf)
        {
            KursOsnovno k = new KursOsnovno();
            k.Naziv = naziv;
            k.Jezik=jezik;
            k.KratakOpis=kratakOpis;

            var filter_idKat = Builders<Kategorija>.Filter.Eq("Id", ObjectId.Parse(idKat));
            var kategorija = KategorijaCollection.Find(filter_idKat).FirstOrDefault();
            if(kategorija!=null)
            {
                var katId=ObjectId.Parse(idKat);
                k.Kategorija=new MongoDBRef("Kategorija", katId);
            }

            //k.Kategorija=new MongoDBRef(kategorija);
            
            // var filter = Builders<Kategorija>.Filter.Eq(x => x._id, (BsSo)idKat);
            // var kat=KategorijaCollection.Find(k=>k._id==idKat).FirstOrDefault();
            // var kateg=KategorijaCollection.Find(Query.EQ("_id", BsonValue.Create(idKat)));
            // k.Kategorija.Id=ObjectId(idKat);
            // k.Profesor.Id=ObjectId(idProf);

            KursCollection.InsertOne(k);
            kategorija.Kursevi.Add(k.Kategorija);

            return Ok("Dodat kurs");

        }
      
        [HttpGet]
        [Route("PreuzmiSveKurseveKategorije/{idKat}")]
        public async Task<ActionResult> PreuzmiSveKurseveKategorije(string idKat)
        {
             /* var query1 = from kurs in KursCollection.AsQueryable<KursOsnovno>()
                        where kurs.Kategorija._id == idKat
                        select kurs;*/
            var ret=KursCollection.Find(k=>k.Kategorija.Id==idKat).ToList();
            return Ok(ret);
           
            
        }

        
    }
}