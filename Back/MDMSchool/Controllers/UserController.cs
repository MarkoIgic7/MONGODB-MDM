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
    public class UserController : ControllerBase
    {
        private IMongoCollection<User> UserCollection;
        public UserController(IMongoClient mc)
        {
            var db = mc.GetDatabase("ProjekatMongo");
            var collection = db.GetCollection<User>("Korisnici");
            UserCollection = collection;
        }
        [HttpPost]
        [Route("Register/{username}/{password}")]
        public async Task<ActionResult> Register(string username,string password)
        {
            var user = UserCollection.Find(p => p.Mail==username).FirstOrDefault();
            if(user==null)
            {
                //dodaj ga
                User u = new User();
                u.Mail = username;
                u.Password = password;
                UserCollection.InsertOne(u);
                return Ok(u);
            }
            else
            {
                return BadRequest("Vec je registrovan korisnik za unertim mail-om");
            }
            
        }
        [HttpGet]
        [Route("Login/{username}/{password}")]
        public async Task<ActionResult> Login(string username,string password)
        {
            var m = UserCollection.Find(q => q.Mail==username).FirstOrDefault();
            if(m==null)
            {
                return BadRequest("Ne postoji korisnik sa unetim mail-om");
            }
            else if(m.Password!=password)
            {
                return BadRequest("Navlidan password");
            }
            else
            {
                return Ok(m);
            }

        }
    }
}