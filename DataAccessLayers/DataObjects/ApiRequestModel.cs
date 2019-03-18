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
        public string Vin { get; set; }
        public string VehicleClass { get; set; }
        public string Manufacturer { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int? Year { get; set; }
        public string EngineType { get; set; }
        public string EngineVinCode { get; set; }
        public string Transmission;
        public string AAIA { get; set; }
        public int CurrentMileage { get; set; } = 0;
        public string RawToolPayload { get; set; }
        public string ReportID { get; set; }
    }

}
