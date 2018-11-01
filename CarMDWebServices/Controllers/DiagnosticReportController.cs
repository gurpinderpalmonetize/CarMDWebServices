using DataAccessLayers.DataObjects;
using DataAccessLayers.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarMDWebServices.Controllers
{
    
    public class DiagnosticReportController : ApiController
    {
        DiagnosticReportService _diagnosticReportService = new DiagnosticReportService();

        [HttpPost]
        [Route("api/DiagnosticReport/GetTSBCountByVehicle")]
        public HttpResponseMessage GetTSBCountByVehicle(string key, string vin)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }

            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(key, vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetTSBCountByVehicleCount(vehicleInfo.AAIA));
        }


        [HttpPost]
        [Route("api/DiagnosticReport/GetRecallsCountForVehicle")]
        public HttpResponseMessage GetRecallsCountForVehicle(string key, string vin)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }

            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(key, vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetRecallsCountForVehicleByYearMakeModel(Int32.Parse(vehicleInfo.Year), vehicleInfo.Make, vehicleInfo.Model, vehicleInfo.TrimLevel, null, null));
        }

    }
}
