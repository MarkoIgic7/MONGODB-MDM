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
    public class SkolaController : ControllerBase
    {
        private IMongoCollection<Skola> SkolaCollection;
        public SkolaController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Skola>("Skola");
            SkolaCollection = collection;
        }
        [HttpPost]
        [Route("DodajSkolu/{naziv}/{lokacija}/{kontakt}/{opis}")]
        public async Task<ActionResult> DodajSkolu(String naziv,String lokacija,String kontakt,String opis)
        {
            Skola s = new Skola();
            s.Kontakt = kontakt;
            s.Naziv = naziv;
            s.Lokacija = lokacija;
            s.Opis = opis;

            SkolaCollection.InsertOne(s);

            return Ok("Dodata skola");


        }
      
        [HttpGet]
        [Route("PreuzmiSkolu")]
        public async Task<ActionResult> PreuzmiSkolu()
        {
            var skola=SkolaCollection.Find(_=>true).FirstOrDefault();
            if(skola!=null)
            {
                return Ok(skola);
            }
            else{
                return BadRequest("Ne postoji skola u bazi!");
            }
            
        }

        [HttpDelete]
        [Route("ObrisiSkolu")]
        public async Task<ActionResult> ObrisiSkolu()
        {
            SkolaCollection.DeleteOne(_=>true);
            return Ok("Obrisana skola");
        }
        
    }
}