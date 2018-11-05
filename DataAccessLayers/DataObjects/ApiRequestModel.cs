using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
   public class ApiRequestModel
    {
        public string key { get; set; }
        public string UserId { get; set; }
        public string vin { get; set; }
        public string rawToolPayload { get; set; }
        public string reportID { get; set; }
        public int  vehicleMileage { get; set; }
        public string createdDateTime { get; set; }
    }

    public class RequestModel
    {
        public string key { get; set; }
        public string make { get; set; }
        public string dtcCode { get; set; }
    }

    public class Request
    {
        public string Key { get; set; }
        public string Vin { get; set; }
        public string VehicleMileage { get; set; }
    }
    public class VehicleRequest 
    {
        public string Key { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Region { get; set; }
        //public int PageSize { get; set; }
        //public int CurrentPage { get; set; }
        //public int UserId { get; set; }
        public string VIN;
        public string VehicleClass;
        public string Manufacturer;
        public string Make;
        public string Model;
        public string Trim;
        public int? Year;
        public string EngineType;
        public string EngineVinCode;
        public string Transmission;
        public string AAIA;
        public int? CurrentMileage;
    }

}
