using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.models
{
    public class StationOwner
    {
        public ObjectId Id { get; set; }

        public string StationOwnerName { get; set; }

        public string StationOwnerPhoneNo { get; set; }

        public string StationOwnerPassword { get; set; }
    }
}
