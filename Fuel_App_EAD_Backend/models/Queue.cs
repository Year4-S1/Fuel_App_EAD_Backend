using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers.models
{
    public class Queue
    {
        public ObjectId Id { get; set; }

        public string CustomerId { get; set; }

        public string StationId { get; set; }

        public string QueueDate { get; set; }

        public string QueueArrivalTime { get; set; }

        public string QueueDepatureTime { get; set; }

        public string VehicleType { get; set; }

        public string FuelType { get; set; }

        public string Status { get; set; }


    }
}
