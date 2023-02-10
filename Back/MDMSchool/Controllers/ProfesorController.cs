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
        [Route("DodajProfesora/{ime}/{prezime}/{podaci}/{strucnaSprema}")]
        public async Task<ActionResult> DodajProfesora(String ime, String prezime, String podaci, String strucnaSprema)
        {
            Profesor k = new Profesor();
            k.Ime = ime;
            k.Prezime=prezime;
            k.Podaci=podaci;
            k.StrucnaSprema=strucnaSprema;
            k.Kursevi=new List<KursOsnovno>();

            ProfesorCollection.InsertOne(k);

            return Ok(k);

        }
      
        [HttpGet]
        [Route("PreuzmiSveProfesore")]
        public async Task<ActionResult> PreuzmiSveProfesore()
        {
            var profesori=ProfesorCollection.Find(_=>true).ToList();
            if(profesori.Count!=0)
            {
                return Ok(profesori);
            }
            else{
                return BadRequest("Ne postoje profesori u bazi!");
            }
            
        }

        
    }
}