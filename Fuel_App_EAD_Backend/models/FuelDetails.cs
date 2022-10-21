using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers.models
{
    public class FuelDetails
    {
        public ObjectId Id { get; set; }

        public string StationId { get; set; }

        public string FuelType { get; set; }

        public Boolean FuelAvailability { get; set; }

        public float FuelAmount { get; set; }
    }
}
