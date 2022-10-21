using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuel_App_EAD_Backend.Controllers.models
{
    public class User
    {
        public ObjectId Id { get; set; }
       
        public string UserName { get; set; }

        public string UserPhoneNo { get; set; }

        public string UserPassword { get; set; }

        public string UserType { get; set; }

        public Boolean LoginStatus { get; set; }

    }
}
