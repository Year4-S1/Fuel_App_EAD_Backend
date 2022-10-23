using DnsClient;
using Fuel_App_EAD_Backend.Controllers.models;
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
    public class StationController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public StationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getall")]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost("post")]
        public JsonResult Post(Station station)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").InsertOne(station);

            return new JsonResult(station);
        }

        [HttpGet("search")]
        public JsonResult SearchStation(SearchStation searchstation)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            //check if the stain name is not null
            if (searchstation.StationName != null)
            {           
                var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").Find(station => station.StationName.ToLower() == searchstation.StationName.ToLower()).ToList();

                return new JsonResult(dbList);
            }
            //check if the station location is not null
            else if (searchstation.StationLocation != null)
            {             
                var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").Find(station => station.StationLocation.ToLower() == searchstation.StationLocation.ToLower()).ToList();
                return new JsonResult(dbList);
            }
            else {
                return new JsonResult("Please enter a value to search");
            }            
        }
    }
}
