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

        #region GetTSBCountByVehicle
        [HttpPost]
        [Route("api/DiagnosticReport/GetTSBCountByVehicle")]
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

        #region GetRecallsCountForVehicle
        [HttpPost]
        [Route("api/DiagnosticReport/GetRecallsCountForVehicle")]
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

        #region LogDiagnosticReport
        [HttpPost]
        [Route("api/DiagnosticReport/LogDiagnosticReport")]
        public HttpResponseMessage LogDiagnosticReport(ApiRequestModel apiRequest)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(apiRequest.key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }


            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(apiRequest.key, apiRequest.vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }
            apiRequest.vin = vehicleInfo.VIN;
            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.LogDiagnosticReportWithMileage(apiRequest));
        }
        #endregion

        #region DiagnosticWithMileage
        [HttpPost]
        [Route("api/DiagnosticReport/DiagnosticWithMileage")]

        public HttpResponseMessage DiagnosticWithMileage(ApiRequestModel apiRequest)
        {
            var GetTSBCountByVehicle = _diagnosticReportService.ValidateKeyAndLogTransaction(apiRequest.key);

            if (GetTSBCountByVehicle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, GetTSBCountByVehicle);
            }


            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(apiRequest.key, apiRequest.vin);

            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);
            }
            apiRequest.vin = vehicleInfo.VIN;
            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetMileage(apiRequest));

        }

        #endregion

        #region GetDTCLibraryErrorCodeWithLaymensTerms
        [HttpPost]
        [Route("api/DiagnosticReport/GetDTCLibraryErrorCodeWithLaymensTerms")]

        public HttpResponseMessage GetDTCLibraryErrorCodeWithLaymensTerms(RequestModel apiRequest)
        {

            var vehicleInfo = _diagnosticReportService.GetVehicleInfo(apiRequest.key);

            if (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, vehicleInfo.ValidationFailures);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetByDtcAndMake(apiRequest));

        }
        #endregion

    }
}
