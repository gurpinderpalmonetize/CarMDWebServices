using DataAccessLayers.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class VehicleInfo
    {
        public VehicleInfo()
        {
        }
        
        public Guid? VehicleId;
        public bool IsValid = false;
        public ValidationFailure[] ValidationFailures = null;
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

        public RecallInfo[] NewRecallsToAlert { get; set; }

        public TSBInfo[] NewTSBsToAlert { get; set; }

        public ScheduleMaintenanceServiceInfo[] ScheduleMaintenanceServices = null;
        
        public string ModelImageFileUrl;

        /// <summary>
        /// Gets the <see cref="VehicleInfo"/> result from the Polk vehicle.
        /// </summary>
        /// <param name="registry"><see cref="Registry"/> currently in use.</param>
        /// <param name="v"><see cref="VehicleInfo"/> object to populate</param>
        /// <param name="polkVehicle">The <see cref="PolkVehicle"/> object.</param>
        /// <param name="vin">The <see cref="string"/> VIN used to decode the Polk vehicle.</param>
        /// <returns><see cref="VehicleInfo"/> vehicle info object populated from the PolkVehicle object supplied.</returns>
        protected static internal void PopulateVehicleInfoFromPolkVehicle( VehicleInfo v, PolkVehicleYmme polkVehicle, string vin)
        {
            v.IsValid = (polkVehicle != null);

            if (polkVehicle != null)
            {
                v.VIN = vin;
                v.ManufacturerName = polkVehicle.Manufacturer;
                v.Make = polkVehicle.Make;
                v.Year = polkVehicle.Year.ToString();
                v.Model = polkVehicle.Model;
                v.Transmission = polkVehicle.Transmission;
                v.EngineType = polkVehicle.EngineType;
                //v.BodyType = polkVehicle.BodyType;
                //v.Series = polkVehicle.Series;

                v.TrimLevel = polkVehicle.Trim;
                v.EngineVINCode = polkVehicle.EngineVinCode;
                v.AAIA = polkVehicle.AAIA;
              //  v.ModelImageFileUrl = polkVehicle.GetModelImageUrl(Global.PolkVehicleImageRootUrl);

            }
        }




        /// <summary>
        /// Populates the vehicle from the <see cref="Innova.Vehicles.Vehicle"/> object.
        /// </summary>
        /// <param name="v"><see cref="VehicleInfo"/> object to populate</param>
        /// <param name="innovaVehicle"><see cref="Innova.Vehicles.Vehicle"/> innova vehicle</param>
        /// <param name="usePolkData">A <see cref="bool"/> to indicate if Polk YMME values should be used to populate the object.</param>
        protected static internal void PopulateVehicleInfoFromInnovaVehicle(VehicleInfo v, Vehicle innovaVehicle, bool usePolkData)
        {

//            v.VehicleId = innovaVehicle.Id;
//            v.IsValid = true;
//            v.VIN = innovaVehicle.Vin;

//            if (usePolkData && innovaVehicle.PolkVehicleYMME != null)
//            {
//                v.ManufacturerName = innovaVehicle.PolkVehicleYMME.Manufacturer;
//                //v.ManufacturerNameAlt = innovaVehicle.PolkVehicleYMME.ManufacturerNameAlt;
//                v.Make = innovaVehicle.PolkVehicleYMME.Make;
//                v.Year = innovaVehicle.PolkVehicleYMME.Year.ToString();
//                v.Model = innovaVehicle.PolkVehicleYMME.Model;
//                v.Transmission = innovaVehicle.PolkVehicleYMME.Transmission;
//                v.EngineType = innovaVehicle.PolkVehicleYMME.EngineType;
//#if (!AUTOZONE)
//                v.TrimLevel = innovaVehicle.PolkVehicleYMME.Trim;
//                v.EngineVINCode = innovaVehicle.PolkVehicleYMME.EngineVinCode;
//                //v.BodyType = innovaVehicle.PolkVehicleYMME.BodyCode;
//                v.AAIA = innovaVehicle.PolkVehicleYMME.AAIA;
//#endif
//            }
//            else
//            {
//                v.ManufacturerName = innovaVehicle.ManufacturerName;
//                v.Make = innovaVehicle.Make;
//                v.Year = innovaVehicle.Year.ToString();
//                v.Model = innovaVehicle.Model;
//                v.Transmission = innovaVehicle.TransmissionControlType;
//                v.EngineType = innovaVehicle.EngineType;
//#if (!AUTOZONE)
//                v.ManufacturerNameAlt = innovaVehicle.ManufacturerNameAlt;
//                v.TrimLevel = innovaVehicle.TrimLevel;
//                v.EngineVINCode = innovaVehicle.EngineVINCode;
//                v.BodyType = innovaVehicle.BodyCode;
//                v.AAIA = innovaVehicle.AAIA;
//#endif
//            }

//#if (!AUTOZONE)
//            if (!innovaVehicle.Mileage.IsNull)
//            {
//                v.Mileage = innovaVehicle.Mileage.Value;
//            }
//            if (!innovaVehicle.MileageLastRecordedDateTimeUTC.IsNull)
//            {
//                v.MileageLastRecordedDateTimeUTC = innovaVehicle.MileageLastRecordedDateTimeUTC.Value;
//            }
//            v.SendNewRecallAlerts = innovaVehicle.SendNewRecallAlerts;
//            v.SendNewTSBAlerts = innovaVehicle.SendNewTSBAlerts;
//            v.SendScheduledMaintenanceAlerts = innovaVehicle.SendScheduledMaintenanceAlerts;
//#endif
        }
    }
}
