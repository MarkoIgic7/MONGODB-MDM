using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Driver;

namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupaController : ControllerBase
    {
        private IMongoCollection<Grupa> GrupaCollection;
        private IMongoCollection<KursOsnovno> KursCollection;
        
        public GrupaController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Grupa>("Grupa");
            GrupaCollection = collection;
            var collection2 = db.GetCollection<KursOsnovno>("Kurs");
            KursCollection = collection2;
        }
        [HttpPost]
        [Route("DodajGrupu/{naziv}/{tbr}/{maxbr}/{idKursa}")]
        public async Task<ActionResult> DodajGrupu(String naziv, int tbr, int maxbr, String idKursa)
        {
            var k = KursCollection.Find(k => k.Id == idKursa).FirstOrDefault();
            Grupa g = new Grupa();
            g.MaximalniBroj = maxbr;
            g.TrenutniBroj = tbr;
            g.Naziv = naziv;
            g.OsnovnoKurs = k;
            GrupaCollection.InsertOne(g);

            k.Grupe.Add(g);
            return Ok(g);

        }
    }
}