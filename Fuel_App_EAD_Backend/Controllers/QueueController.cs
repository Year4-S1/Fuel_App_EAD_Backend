﻿using Fuel_App_EAD_Backend.Controllers.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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

            //setting the current date to the QueueDate
            queue.QueueDate = DateTime.Now.ToString("dd/MM/yyyy");
            //setting the current time to the QueueArrivalTime
            queue.QueueArrivalTime = DateTime.Now.ToString("HH:mm:ss");
            queue.QueueDepatureTime = "";

            dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").InsertOne(queue);

            return new JsonResult(queue);
        }

        [HttpPut("departure/time/update/{id}")]
        public JsonResult UpdateDepatureTime(String id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));        
           
            //converting the string id to a mongoose bson objectId
            var queueId = new ObjectId(id);
            //filtering by the queueId
            var filter = Builders<Queue>.Filter.Eq("_id", queueId);
            //updating the queueDepartureTome and the status of a queue
            var update = Builders<Queue>.Update.Set("QueueDepatureTime", DateTime.Now.ToString("HH:mm:ss")).Set("Status", "Exit");
            dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").UpdateOne(filter, update);
            //filtering the updated document
            var updated_logout = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").Find(queue => queue.Id == queueId).ToList();

            return new JsonResult(updated_logout);
        
        }

        [HttpGet("count/vehicle/type/{id}")]
        public JsonResult GetQueueLenghtByVehicleType(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));           

            //counting the vehicles in the queue per station
            int QueueCarCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "car".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueMotorCycleCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "motor cycle".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueThreeWheelersCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "three-wheelers".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueVanCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "van".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueLorryCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "lorry".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueBusCount = dbClient.GetDatabase("fuelappdb").GetCollection<Queue>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "bus".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.QueueDate == DateTime.Now.ToString("dd/MM/yyyy"));                     

            //adding the vehicle counts to the dictionary
            Dictionary<string , int> QueueVehicleCountN = new Dictionary<string, int>();
            QueueVehicleCountN.Add("Car", QueueCarCount);
            QueueVehicleCountN.Add("MotorCycle", QueueMotorCycleCount);
            QueueVehicleCountN.Add("Three-Wheelers", QueueThreeWheelersCount);
            QueueVehicleCountN.Add("Van", QueueVanCount);
            QueueVehicleCountN.Add("Lorry", QueueLorryCount);
            QueueVehicleCountN.Add("Bus", QueueBusCount);

            return new JsonResult(QueueVehicleCountN);
        }
    }
}
