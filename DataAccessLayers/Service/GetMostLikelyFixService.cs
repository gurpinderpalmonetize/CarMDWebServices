using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;
using System;

namespace DataAccessLayers.Service
{
    public class GetMostLikelyFixService
    {
        public innovaEntities _innovaEntities;

        public GetMostLikelyFixService() { }
        public GetMostLikelyFixService(innovaEntities innovaEntities)
        {
            _innovaEntities = innovaEntities;
        }

        public DiagReportInfo GetMostLikelyFixForVehicle(VehicleRequest apiRequest, VehicleInfoModel vehicleInfo)
        {
            DiagReportInfo drInfo = new DiagReportInfo();
            drInfo = this.GetDiagnosticReport(
                                    apiRequest.Key,
                                    "GetMostLikelyFix",
                                    apiRequest.ReportID,
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    vehicleInfo.VIN,
                                    false,
                                    vehicleInfo.Mileage,
                                    "",
                                    "false",
                                    "false",
                                    "false",
                                    "false",
                                    "false",
                                    int.MinValue,
                                    int.MinValue,
                                    null,
                                    apiRequest.RawToolPayload,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    "false",
                                    false);


            return new DiagReportInfo();
        }

        protected DiagReportInfo GetDiagnosticReport(
    string key,
    string methodInvoked,
    string externalSystemReportId,
    string externalSystemUserIdGuidString,
    string externalSystemUserFirstName,
    string externalSystemUserLastName,
    string externalSystemUserEmailAddress,
    string externalSystemUserPhoneNumber,
    string externalSystemUserRegion,
    string vin,
    bool validateVin,
    int mileage,
    string transmission,
    string includeRecallsForVehicle,
    string includeTSBsForVehicleAndMatchingErrorCodes,
    string includeTSBCountForVehicle,
    string includeNextScheduledMaintenance,
    string includeWarrantyInfo,
    int softwareTypeInt,
    int toolTypeFormatInt,
    string diagnosticReportIdGuidString,
    string rawUpload,
    string rawFreezeFrameDataString,
    string rawMonitorsDataString,
    string pwrPrimaryDtc,
    string pwrStoredDtcCommaSeparatedList,
    string pwrPendingDtcCommaSeparatedList,
    string pwrPermanentDtcCommaSeparatedList,
    string obd1StoredCodesCommaSeparatedList,
    string obd1PendingCodesCommaSeparatedList,
    string absStoredCodesCommaSeparatedList,
    string absPendingCodesCommaSeparatedList,
    string srsStoredCodesCommaSeparatedList,
    string srsPendingCodesCommaSeparatedList,
    string pwrFixNotFoundFixPromisedByDateTimeUTCString,
    string obd1FixNotFoundFixPromisedByDateTimeUTCString,
    string absFixNotFoundFixPromisedByDateTimeUTCString,
    string srsFixNotFoundFixPromisedByDateTimeUTCString,
    string saveReport,
    bool isUpdateNoFix,
    string createdDateTimeUTCString = "",
    string parentDiagnosticReportIdGuidString = "",
    string manualRawFreezeFrameDataString = "",
    bool additionalHelpRequired = false,
    bool isNotifiedRequester = false,
    DateTime? notifiedRequesterDateTimeUTC = null,
    string notifiedRequesterVia = "",
    string note = ""
    )
        {
            DiagReportInfo dr = new DiagReportInfo();
            WebServiceSessionStatus errors = new WebServiceSessionStatus();
            dr.WebServiceSessionStatus = errors;

            /*****************************************************************************
			* Vehicle processing
			* ***************************************************************************/

            PolkVinDecoder polkVinDecoder = new PolkVinDecoder(this.Registry);
            PolkVehicleYMME polkVehicleYMME = null;
            VehicleInfo v = new VehicleInfo();


            //Added on 2017-06-23 8:27 AM by INNOVA Dev Team
            string errMessage = string.Empty;
            try
            {
                //Added on 2017-07-30 11:31 PM by INNOVA Dev Team to check for the requested VIN is a masked VIN or not
                if (polkVinDecoder.IsVinAValidMaskPattern(vin))
                    validateVin = false;

                polkVehicleYMME = polkVinDecoder.DecodeVIN(vin, validateVin);
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
            }

            if (!string.IsNullOrEmpty(errMessage))
                throw new Exception(errMessage);

            if (polkVehicleYMME == null)
            {
                v.ValidationFailures = new ValidationFailure[1];
                v.ValidationFailures[0] = new ValidationFailure();
                v.ValidationFailures[0].Code = "426";
                v.ValidationFailures[0].Description = "The VIN supplied is invalid, you cannot retreive a diagnostic report without a valid vin";
                v.IsValid = false;

                errors.AddValidationFailure("426", v.ValidationFailures[0].Description);
            }
            else
            {
                VehicleInfo.PopulateVehicleInfoFromPolkVehicle(this.Registry, v, polkVehicleYMME, vin);
            }


            //if not valid return here
            if (errors.ValidationFailures.Length > 0)
            {
                this.LogValidationFailures(errors.ValidationFailures, key, methodInvoked, vin, rawUpload);
                return dr;
            }
            else
            {
                return this.GetDiagnosticReport(
                    null, //symptom request
                    key,
                    methodInvoked,
                    externalSystemReportId,
                    externalSystemUserIdGuidString,
                    externalSystemUserFirstName,
                    externalSystemUserLastName,
                    externalSystemUserEmailAddress,
                    externalSystemUserPhoneNumber,
                    externalSystemUserRegion,
                    vin,
                    validateVin,
                    v.AAIA,
					"",
                    v.ManufacturerName,
                    v.Make,
                    v.Year,
                    v.Model,
                    v.TrimLevel,
					"",
                    v.EngineType,
                    v.EngineVINCode,
                    mileage,
                    transmission,
                    includeRecallsForVehicle,
                    includeTSBsForVehicleAndMatchingErrorCodes,
                    includeTSBCountForVehicle,
                    includeNextScheduledMaintenance,
                    includeWarrantyInfo,
                    softwareTypeInt,
                    toolTypeFormatInt,
                    diagnosticReportIdGuidString,
                    rawUpload,
                    rawFreezeFrameDataString,
                    rawMonitorsDataString,
                    pwrPrimaryDtc,
                    pwrStoredDtcCommaSeparatedList,
                    pwrPendingDtcCommaSeparatedList,
                     pwrPermanentDtcCommaSeparatedList,
                    obd1StoredCodesCommaSeparatedList,
                    obd1PendingCodesCommaSeparatedList,
                    absStoredCodesCommaSeparatedList,
                    absPendingCodesCommaSeparatedList,
                    srsStoredCodesCommaSeparatedList,
                    srsPendingCodesCommaSeparatedList,
                    pwrFixNotFoundFixPromisedByDateTimeUTCString,
                    obd1FixNotFoundFixPromisedByDateTimeUTCString,
                    absFixNotFoundFixPromisedByDateTimeUTCString,
                    srsFixNotFoundFixPromisedByDateTimeUTCString,
                    saveReport,
                    isUpdateNoFix,
                    createdDateTimeUTCString,
                    //New params - Nam added 1/9/2017 for new OBDFIX
                    parentDiagnosticReportIdGuidString: parentDiagnosticReportIdGuidString,
                    manualRawFreezeFrameDataString: manualRawFreezeFrameDataString,
                    additionalHelpRequired: additionalHelpRequired,
                    isNotifiedRequester: isNotifiedRequester,
                    notifiedRequesterDateTimeUTC: notifiedRequesterDateTimeUTC,
                    notifiedRequesterVia: notifiedRequesterVia,
                    note: note
                    );
            }
        }
    }
}
