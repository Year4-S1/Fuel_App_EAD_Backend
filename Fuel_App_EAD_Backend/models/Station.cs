using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers.models
{
    public class Station
    {
        public ObjectId Id { get; set; }

        public string StationOwnerId { get; set; }

        public string StationName { get; set; }

        public string StationLocation { get; set; }

    }
}
