using Fuel_App_EAD_Backend.Controllers.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public QueueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getall")]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost("post")]
        public JsonResult Post(Queue queue)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            queue.QueueDate = DateTime.Now.ToString("dd/MM/yyyy");
            queue.QueueArrivalTime = DateTime.Now.ToString("HH:mm:ss");
            queue.QueueDepatureTime = "";

            dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").InsertOne(queue);

            return new JsonResult("Added Successfully");
        }

        [HttpPut("departure/time/update/{id}")]
        public JsonResult UpdateDepatureTime(String id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));        
           
            var queueId = new ObjectId(id);
            var filter = Builders<Queue>.Filter.Eq("_id", queueId);
            var update = Builders<Queue>.Update.Set("QueueDepatureTime", DateTime.Now.ToString("HH:mm:ss")).Set("Status", "Exit");
            dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").UpdateOne(filter, update);
            var updated_logout = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").Find(queue => queue.Id == queueId).ToList();

            return new JsonResult(updated_logout);
        
        }
    }
}
