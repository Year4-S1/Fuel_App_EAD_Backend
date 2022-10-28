using DnsClient;
using Fuel_App_EAD_Backend.Controllers.models;
using Fuel_App_EAD_Backend.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections;
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

        [HttpGet("search/{station}")]
        public JsonResult SearchStation(string station)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var stationValue = station;

            //check if the stain name is not null
            if (station != null)
            {           
                var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").Find(station => station.StationName.ToLower() == stationValue.ToLower() || station.StationLocation.ToLower() == stationValue.ToLower()).ToList();

                return new JsonResult(dbList);
            }            
            else {
                return new JsonResult("Please enter a value to search");
            }            
        }

        [HttpGet("getstation/{id}")]
        public JsonResult UpdateStationDetails(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var ownerId = id;

            //check if the stain name is not null
           
            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").Find(station => station.StationOwnerId.ToLower() == ownerId.ToLower() ).ToList();

                //filtering by the queueId
               // var filter = Builders<Station>.Filter.Eq("StationOwnerId", ownerId);
                //updating the queueDepartureTome and the status of a queue
               // var update = Builders<Station>.Update.Set("QueueDepatureTime", DateTime.Now.ToString("HH:mm:ss")).Set("Status", "Exit");
               // dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").UpdateOne(filter, update);
                //filtering the updated document
                //var updated_logout = dbClient.GetDatabase("fuelappdb").GetCollection<Station>("station").Find(station => station.Id == queueId).ToList();



             return new JsonResult(dbList[0]);
           
        }
    }
}
