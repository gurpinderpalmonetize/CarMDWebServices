using DataAccessLayers.DataObjects;
using DataAccessLayers.Service;
using System;
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
            var key = Request.Headers.GetValues("Token");
            var vaildatekey = _diagnosticReportService.vaildatekey(string.Join("", key));
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(request.Vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetTSBCountByVehicleCount(vehicleInfo.AAIA));
        }
        #endregion

        #region GetRecallsCountForVehicle
        [HttpPost]
        [Route("api/DiagnosticReport/GetRecallsCountForVehicle")]
        public HttpResponseMessage GetRecallsCountForVehicle(Request request)
        {
            var key = Request.Headers.GetValues("Token");
            var vaildatekey = _diagnosticReportService.vaildatekey(string.Join("", key));
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(request.Vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetRecallsCountForVehicleByYearMakeModel(Int32.Parse(vehicleInfo.Year), vehicleInfo.Make, vehicleInfo.Model, vehicleInfo.TrimLevel, null, null));
        }
        #endregion

        #region LogDiagnosticReport
        [HttpPost]
        [Route("api/DiagnosticReport/LogDiagnosticReport")]
        public HttpResponseMessage LogDiagnosticReport(ApiRequestModel apiRequest)
        {
            var key = Request.Headers.GetValues("Token");
            var vaildatekey = _diagnosticReportService.vaildatekey(string.Join("", key));
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);


            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            apiRequest.vin = vehicleInfo.VIN;
            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.LogDiagnosticReportWithMileage(apiRequest));
        }
        #endregion

        #region DiagnosticWithMileage
        [HttpPost]
        [Route("api/DiagnosticReport/DiagnosticWithMileage")]

        public HttpResponseMessage DiagnosticWithMileage(ApiRequestModel apiRequest)
        {
            var key = Request.Headers.GetValues("Token");
            var vaildatekey = _diagnosticReportService.vaildatekey(string.Join("", key));
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            var vehicleInfo = _diagnosticReportService.GetVehicleInfoByVin(apiRequest.vin);
            if (vehicleInfo == null || (vehicleInfo.ValidationFailures != null && vehicleInfo.ValidationFailures.Length > 0))
                return Request.CreateResponse(HttpStatusCode.NoContent, vehicleInfo.ValidationFailures);

            apiRequest.vin = vehicleInfo.VIN;
            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetMileage(apiRequest));

        }

        #endregion

        #region GetDTCLibraryErrorCodeWithLaymensTerms
        [HttpPost]
        [Route("api/DiagnosticReport/GetDTCLibraryErrorCodeWithLaymensTerms")]

        public HttpResponseMessage GetDTCLibraryErrorCodeWithLaymensTerms(RequestModel apiRequest)
        {
            var key = Request.Headers.GetValues("Token");
            var vaildatekey = _diagnosticReportService.vaildatekey(string.Join("", key));
            if (vaildatekey.ValidationFailures != null && vaildatekey.ValidationFailures.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, vaildatekey.ValidationFailures);

            return Request.CreateResponse(HttpStatusCode.OK, _diagnosticReportService.GetByDtcAndMake(apiRequest));

        }
        #endregion

    }
}
