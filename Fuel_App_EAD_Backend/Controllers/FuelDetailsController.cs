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
            //var dbList = dbClient.GetDatabase("fuelappdb").GetCollection<FuelDetails>("fueldetail").Find(fueldetail => fueldetail._ == login.PhoneNo).ToList();

            return new JsonResult("Added Successfully");
        }
    }
}
