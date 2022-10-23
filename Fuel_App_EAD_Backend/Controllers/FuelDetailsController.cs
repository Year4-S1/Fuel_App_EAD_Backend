using DnsClient;
using Fuel_App_EAD_Backend.Controllers.models;
using Fuel_App_EAD_Backend.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fuel_App_EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelDetailsController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public FuelDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("post")]
        public JsonResult Post(FuelDetails fuelDetails)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            dbClient.GetDatabase("fuelappdb").GetCollection<FuelDetails>("fueldetail").InsertOne(fuelDetails);

            return new JsonResult(fuelDetails);
        }

        [HttpGet("getfuel/perstation/{id}")]
        public JsonResult GetFuelDetailsPerStation(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var per_Station_fuel_list = dbClient.GetDatabase("fuelappdb").GetCollection<FuelDetails>("fueldetail").Find(fueldetail => fueldetail.StationId == id).ToList();

            return new JsonResult(per_Station_fuel_list);
        }

        [HttpPut("update/{id}")]
        public JsonResult UpdateFuelDetails(string id, FuelDetails fuelDetails)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));

            var fuelId = new ObjectId(id);
            //filter by fuelId
            var filter = Builders<FuelDetails>.Filter.Eq("_id", fuelId);
            //update fuel status and amount
            var update = Builders<FuelDetails>.Update.Set("FuelAvailability", fuelDetails.FuelAvailability).Set("FuelAmount", fuelDetails.FuelAmount);
            dbClient.GetDatabase("fuelappdb").GetCollection<FuelDetails>("fueldetail").UpdateOne(filter, update);
            var updated_fuel = dbClient.GetDatabase("fuelappdb").GetCollection<FuelDetails>("fueldetail").Find(fueldetail => fueldetail.Id == fuelId).ToList();

            return new JsonResult(updated_fuel);
        }
    }
}