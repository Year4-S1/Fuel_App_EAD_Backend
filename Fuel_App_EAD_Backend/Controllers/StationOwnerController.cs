using Fuel_App_EAD_Backend.models;
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
    public class StationOwnerController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public StationOwnerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getall")]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<StationOwner>("stationowner").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost("post")]
        public JsonResult Post(StationOwner stationowner)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            dbClient.GetDatabase("fuelappdb").GetCollection<StationOwner>("stationowner").InsertOne(stationowner);

            return new JsonResult("Added Successfully");
        }
    }
}
