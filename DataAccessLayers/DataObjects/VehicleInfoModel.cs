using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class VehicleInfoModel
    {
        public Guid? VehicleId { get; set; }
        public bool IsValid { get; set; }
        public ValidationFailureModel[] ValidationFailures { get; set; }
        public string VIN { get; set; } 
        public string ManufacturerName { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EngineType { get; set; }
        public string Transmission { get; set; }
        public string ManufacturerNameAlt { get; set; }
        public string EngineVINCode { get; set; }
        public string BodyType { get; set; }
        public string TrimLevel { get; set; }
        public string Series { get; set; }
        public string AAIA { get; set; }
        public int? Mileage { get; set; }
        public DateTime? MileageLastRecordedDateTimeUTC { get; set; }
        public bool SendScheduledMaintenanceAlerts { get; set; }
        public bool SendNewTSBAlerts { get; set; }
        public bool SendNewRecallAlerts { get; set; }
        public string ModelImageFileUrl { get; set; }

    }
}
