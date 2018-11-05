using DataAccessLayers.DataObjects;
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
            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(apiRequest.Key);
            if (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, vehicleInfo.ValidationFailures);
            }

            var GetVehicleInfoByVin = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.VIN);
            if (GetVehicleInfoByVin == null || (GetVehicleInfoByVin.ValidationFailures != null && GetVehicleInfoByVin.ValidationFailures.Length > 0))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetVehicleInfoByVin.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK,_diagnosticReportService.GetDLCLocation(apiRequest));
        }
        #endregion

        #region GetRecallsCountForVehicle
        [HttpPost]
        [Route("api/AutoZone/GetRecallsCountForVehicle")]
        public HttpResponseMessage GetRecallsCountForVehicle(Request request)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(request.Key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }

            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(request.Key, request.Vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetRecallsCountForVehicleByYearMakeModel(Int32.Parse(vehicleInfo.Year), vehicleInfo.Make, vehicleInfo.Model, vehicleInfo.TrimLevel, null, null));
        }
        #endregion

        #region GetTSBCountByVehicle
        [HttpPost]
        [Route("api/AutoZone/GetTSBCountByVehicle")]
        public HttpResponseMessage GetTSBCountByVehicle(Request request)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(request.Key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }

            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(request.Key, request.Vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetTSBCountByVehicleCount(vehicleInfo.AAIA));
        }
        #endregion

        #region GetScheduledMaintenanceNextServiceForVehicle
        [HttpPost]
        [Route("api/AutoZone/GetScheduledMaintenanceNextServiceForVehicle")]
        public HttpResponseMessage GetScheduledMaintenanceNextServiceForVehicle(Request apiRequest)
        {
            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(apiRequest.Key);
            if (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, vehicleInfo.ValidationFailures);
            }

            var GetVehicleInfoByVin = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.Vin);
            if (GetVehicleInfoByVin == null || (GetVehicleInfoByVin.ValidationFailures != null && GetVehicleInfoByVin.ValidationFailures.Length > 0))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetVehicleInfoByVin.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK);// _diagnosticReportService.GetScheduledMaintenanceNextServiceForVehicle(apiRequest));
        }
        #endregion
    }
}
