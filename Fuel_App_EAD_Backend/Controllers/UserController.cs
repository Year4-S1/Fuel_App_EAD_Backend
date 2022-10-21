using Fuel_App_EAD_Backend.Controllers.models;
using Fuel_App_EAD_Backend.models;
using Fuel_App_EAD_Backend.PasswordHashing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getall/users")]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<User>("user").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost("post/user")]
        public JsonResult Post(User user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            // Hash function call
            user.UserPassword = SecurePasswordHasher.Hash(user.UserPassword);
            user.LoginStatus = false;

            dbClient.GetDatabase("fuelappdb").GetCollection<User>("user").InsertOne(user);

            return new JsonResult("Added Successfully");
        }

        [HttpPost("login")]
        public JsonResult Login(Login login)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));           


            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<User>("user").Find(user => user.UserPhoneNo == login.PhoneNo).ToList();

            // Verify
            var result = SecurePasswordHasher.Verify(login.Password, dbList[0].UserPassword);

            if (result == true)
            {
                User user = dbList[0];
                var filter = Builders<User>.Filter.Eq("_id", user.Id);
                var update = Builders<User>.Update.Set("LoginStatus", true );
                dbClient.GetDatabase("fuelappdb").GetCollection<User>("user").UpdateOne(filter, update);
                var updated_login = dbClient.GetDatabase("fuelappdb").GetCollection<User>("user").Find(user => user.UserPhoneNo == login.PhoneNo).ToList();


                return new JsonResult(updated_login);
            }
            else {
                return new JsonResult("Invalid User");
            }
            
        }
    }
}
