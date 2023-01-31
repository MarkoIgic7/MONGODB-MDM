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
    public class ProfesorController : ControllerBase
    {
        private IMongoCollection<Profesor> ProfesorCollection;
        public ProfesorController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<Profesor>("Profesor");
            ProfesorCollection = collection;
        }

        [HttpPost]
        [Route("DodajProfesor/{ime}/{prezime}/{podaci}/{strucnaSprema}")]
        public async Task<ActionResult> DodajProfesor(String ime, String prezime, String podaci, String strucnaSprema)
        {
            Profesor k = new Profesor();
            k.Ime = ime;
            k.Prezime=prezime;
            k.Podaci=podaci;
            k.StrucnaSprema=strucnaSprema;

            ProfesorCollection.InsertOne(k);

            return Ok(k);

        }
      
       /* [HttpGet]
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
            
        }*/

        
    }
}