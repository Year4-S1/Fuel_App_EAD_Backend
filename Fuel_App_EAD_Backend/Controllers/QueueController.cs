using Fuel_App_EAD_Backend.Controllers.models;
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
            queue.QueueTime = DateTime.Now.ToString("HH:mm:ss");

            dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").InsertOne(queue);

            return new JsonResult("Added Successfully");
        }
    }
}
