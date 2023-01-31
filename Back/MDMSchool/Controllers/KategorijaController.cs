using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Driver;

namespace MDMSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KategorijaController : ControllerBase
    {
        private IMongoCollection<Kategorija> KategorijaCollection;
        public KategorijaController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Kategorija>("Kategorija");
            KategorijaCollection = collection;
        }

        [HttpPost]
        [Route("DodajKategoriju/{uzrast}")]
        public async Task<ActionResult> DodajKategoriju(String uzrast)
        {
            Kategorija k = new Kategorija();
            k.Uzrast = uzrast;

            KategorijaCollection.InsertOne(k);

            return Ok("Dodata kategorija");

        }
      
        [HttpGet]
        [Route("PreuzmiSveKategorije")]
        public async Task<ActionResult> PreuzmiSveKategorije()
        {
            var kategorije=KategorijaCollection.Find(_=>true).ToList();
            if(kategorije.Count!=0)
            {
                return Ok(kategorije);
            }
            else{
                return BadRequest("Ne postoje kategorije u bazi!");
            }
            
        }

        
    }
}