﻿using DataAccessLayers.DataObjects;
using DataAccessLayers.Service;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarMDWebServices.Controllers
{
    public class AutoZoneController : ApiController
    {
        DiagnosticReportService _diagnosticReportService = new DiagnosticReportService();

        #region DiagnosticWithMileage
        [HttpPost]
        [Route("api/AutoZone/GetDLCLocation")]
        public HttpResponseMessage GetDLCLocation(VehicleRequest apiRequest)
        {
            var vaildatekey = _diagnosticReportService.vaildatekey(apiRequest.Key);
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.VIN);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NotFound, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetDLCLocation(apiRequest));
        }
        #endregion

        #region GetRecallsCountForVehicle
        [HttpPost]
        [Route("api/AutoZone/GetRecallsCountForVehicle")]
        public HttpResponseMessage GetRecallsCountForVehicle(Request request)
        {
            var vaildatekey = _diagnosticReportService.vaildatekey(request.Key);
            if (vaildatekey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(request.Vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetRecallsCountForVehicleByYearMakeModel(Int32.Parse(vehicleInfo.Year), vehicleInfo.Make, vehicleInfo.Model, vehicleInfo.TrimLevel, null, null));
        }
        #endregion

        #region GetTSBCountByVehicle
        [HttpPost]
        [Route("api/AutoZone/GetTSBCountByVehicle")]
        public HttpResponseMessage GetTSBCountByVehicle(Request request)
        {
            var vaildatekey = _diagnosticReportService.vaildatekey(request.Key);
            if (vaildatekey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(request.Vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetTSBCountByVehicleCount(vehicleInfo.AAIA));
        }
        #endregion

        #region GetScheduledMaintenanceNextServiceForVehicle
        [HttpPost]
        [Route("api/AutoZone/GetScheduledMaintenanceNextServiceForVehicle")]
        public HttpResponseMessage GetScheduledMaintenanceNextServiceForVehicle(Request apiRequest)
        {
            var vaildatekey = _diagnosticReportService.vaildatekey(apiRequest.Key);
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.Vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NotFound, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetScheduledMaintenanceNextServiceForVehicle(Convert.ToInt16(vehicleInfo.Year), vehicleInfo.ManufacturerName, vehicleInfo.Make, vehicleInfo.Model, vehicleInfo.EngineType, vehicleInfo.TrimLevel, vehicleInfo.Transmission, vehicleInfo.EngineVINCode, Convert.ToInt32(apiRequest.VehicleMileage)));
        }
        #endregion
    }
}
