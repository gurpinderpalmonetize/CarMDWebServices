using DataAccessLayers.Common;
using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;
using DataAccessLayers.Repository;
using Innova.Utilities.Shared;
using Innova.Utilities.Shared.Enums;
using Innova.Utilities.Shared.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAccessLayers.Service
{
    public class GetMostLikelyFixService
    {
        public innovaEntities _innovaEntities;
        public DiagnosticReportService _diagnosticReportService;
        public GetMostLikelyFixRepository  _getMostLikelyFixRepository;
        public DiagnosticReport _diagnosticReport;
        public PolkVehicleYmme _objPolkVehicleYmme;
        public Vehicle _Vehicle;

        private const string FLEET_PAYLOAD_PREFIX = "__FLEET_PAYLOAD__";
        private readonly Guid FLEET_TOOL_ID = new Guid("{ED8EB8DA-DFEE-4C42-941C-4F1E997A755E}");
        private readonly Guid AUTOZONE_TOOL_ID = new Guid("{CE2D229A-E2FE-474E-9C91-6BA5DDC68477}");
        private DiagnosticReportFixStatus PwrDiagnosticReportFixStatus = DiagnosticReportFixStatus.FixNotNeeded;
        private DiagnosticReportFixStatus Obd1DiagnosticReportFixStatus = DiagnosticReportFixStatus.FixNotNeeded;
        private DiagnosticReportFixStatus AbsDiagnosticReportFixStatus = DiagnosticReportFixStatus.FixNotNeeded;
        private DiagnosticReportFixStatus SrsDiagnosticReportFixStatus = DiagnosticReportFixStatus.FixNotNeeded;
        StringCollection pwrStoredErrorCodeList = new StringCollection();
        StringCollection pwrPendingErrorCodeList = new StringCollection();
        StringCollection pwrPermanentErrorCodeList = new StringCollection();
        List<string> PwrPendingCodes = new List<string>();
        List<string> PwrStoredCodes = new List<string>();
        List<string> PwrPermanentCodes = new List<string>();
        public bool IsFromPolkMatch { get; set; }
        public bool HasMasterTechsAssigned { get; set; }
        public string absStoredCodesString { get; set; }
        public string srsStoredCodesString { get; set; }
        public string obd1StoredCodesString { get; set; }
        public string AdminUserDeletedBy { get; set; }
        private string masterTechMakesString { get; set; }
        public bool isObjectCreated { get; set; } = false;
        private List<string> symptomGuids = new List<string>();
        private List<string> masterTechMakes { get; set; }
        private List<User> MasterTechsAssigned { get; set; }
        public bool isMasterTechsAssignedDirty = false;

        public bool unableToFindCodeData { get; set; }
        private bool IsObjectDirty { get; set; }

        private bool pwrFixFoundAfterLastFixLookup { get; set; }
        private bool obd1FixFoundAfterLastFixLookup { get; set; }
        private bool absFixFoundAfterLastFixLookup { get; set; }
        private bool srsFixFoundAfterLastFixLookup { get; set; }
        List<DiagnosticReportResultFix> diagnosticReportResultFixes = new List<DiagnosticReportResultFix>();
        List<DiagnosticReportResultErrorCode> diagnosticReportResultErrorCode =   new List<DiagnosticReportResultErrorCode>();
        DiagnosticReportResult diagnosticReportResult = new DiagnosticReportResult();

        string StateCode = "QC"; 
        Currency _currentCurrency;

        private decimal laborRate { get; set; }
        private decimal laborCost { get; set; }
        private decimal partsCost { get; set; }
        private decimal totalCost { get; set; }
        private decimal laborRateInLocalCurrency { get; set; }
        private decimal laborCostInLocalCurrency { get; set; }
        private decimal additionalCostInLocalCurrency { get; set; }
        private decimal partsCostInLocalCurrency { get; set; }
        private decimal totalCostInLocalCurrency { get; set; }
        private DateTime FixProvidedDateTimeUTC { get; set; }
        public bool IsFromVinPowerMatch { get; set; }
        public string bodyCode { get; set; }
        public string ExternalSystemReportId { get; set; }

        public List<string> SymptomGuids
        {
            get
            {
                if (symptomGuids == null)
                {
                    symptomGuids = new List<string>();
                    foreach (SymptomDiagnosticReportItem st in SymptomDiagnosticReportItem)
                    {
                        symptomGuids.Add( st.SymptomId.ToString());
                    }
                }
                return symptomGuids;
            }
        }

        public GetMostLikelyFixService() { }
        public GetMostLikelyFixService(innovaEntities innovaEntities, DiagnosticReportService diagnosticReportService, GetMostLikelyFixRepository GetMostLikelyFixRepository, DiagnosticReport diagnosticReport, PolkVehicleYmme polkVehicleYmme,Vehicle vehicle)
        {
            _innovaEntities = innovaEntities;
            _diagnosticReportService = diagnosticReportService;
            _getMostLikelyFixRepository = GetMostLikelyFixRepository;
            _diagnosticReport = diagnosticReport;
            _objPolkVehicleYmme = polkVehicleYmme;
            _Vehicle = vehicle;
        }

        public DiagReportInfo GetMostLikelyFixForVehicle(VehicleRequest apiRequest)
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
                                    apiRequest.Vin,
                                    false,
                                    0,
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
                        string note = "")
        {
            DiagReportInfo dr = new DiagReportInfo();
            WebServiceSessionStatus errors = new WebServiceSessionStatus();
            dr.WebServiceSessionStatus = errors;

            /*****************************************************************************
			* Vehicle processing
			* ***************************************************************************/

            PolkVehicleYmme polkVehicleYMME = new PolkVehicleYmme();
            VehicleInfo v = new VehicleInfo();

            string errMessage = string.Empty;
            try
            {

                if (_diagnosticReportService.IsVinAValidMaskPattern(vin))
                    validateVin = false;
                polkVehicleYMME = _diagnosticReportService.DecodeVIN(vin, false);
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
                v.VIN = polkVehicleYMME.VinPatternMask;
                v.ManufacturerName = polkVehicleYMME.Manufacturer;
                v.Make = polkVehicleYMME.Make;
                v.Year = polkVehicleYMME.Year.ToString();
                v.Model = polkVehicleYMME.Model;
                v.Transmission = polkVehicleYMME.Transmission;
                v.EngineType = polkVehicleYMME.EngineType;
                v.TrimLevel = polkVehicleYMME.Trim;
                v.EngineVINCode = polkVehicleYMME.EngineVinCode;
                v.AAIA = polkVehicleYMME.AAIA;
                v.ModelImageFileUrl = GlobalModel.PolkVehicleImageRootUrl;
            }


            //if not valid return here
            if (errors.ValidationFailures.Length > 0)
            {
                return dr;
            }
            else
            {
                return this.MasterGetDiagnosticReport(
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
                    parentDiagnosticReportIdGuidString,
                    manualRawFreezeFrameDataString: manualRawFreezeFrameDataString,
                    additionalHelpRequired: additionalHelpRequired,
                    isNotifiedRequester: isNotifiedRequester,
                    notifiedRequesterDateTimeUTC: notifiedRequesterDateTimeUTC,
                    notifiedRequesterVia: notifiedRequesterVia,
                    note: note
                    );
            }
        }

        protected DiagReportInfo MasterGetDiagnosticReport(
                    string symptomRequest = null,
                    string key = null,
                    string methodInvoked = "",
                    string externalSystemReportId = "",
                    string externalSystemUserIdGuidString = "",
                    string externalSystemUserFirstName = "",
                    string externalSystemUserLastName = "",
                    string externalSystemUserEmailAddress = "",
                    string externalSystemUserPhoneNumber = "",
                    string externalSystemUserRegion = "",
                    string vin = "",
                    bool validateVin = false,
                    string aaia = "",
                    string manufacturer = "",
                    string make = "",
                    string year = "",
                    string model = "",
                    string trimLevel = "",
                    string engineType = "",
                    string engineCode = "",
                    int mileage = -1,
                    string transmission = "",
                    string includeRecallsForVehicle = "",
                    string includeTSBsForVehicleAndMatchingErrorCodes = "",
                    string includeTSBCountForVehicle = "",
                    string includeNextScheduledMaintenance = "",
                    string includeWarrantyInfo = "",
                    int softwareTypeInt = -1,
                    int toolTypeFormatInt = -1,
                    string diagnosticReportIdGuidString = "",
                    string rawUpload = "",
                    string rawFreezeFrameDataString = "",
                    string rawMonitorsDataString = "",
                    string pwrPrimaryDtc = "",
                    string pwrStoredDtcCommaSeparatedList = "",
                    string pwrPendingDtcCommaSeparatedList = "",
                    string pwrPermanentDtcCommaSeparatedList = "",
                    string obd1StoredCodesCommaSeparatedList = "",
                    string obd1PendingCodesCommaSeparatedList = "",
                    string absStoredCodesCommaSeparatedList = "",
                    string absPendingCodesCommaSeparatedList = "",
                    string srsStoredCodesCommaSeparatedList = "",
                    string srsPendingCodesCommaSeparatedList = "",
                    string pwrFixNotFoundFixPromisedByDateTimeUTCString = "",
                    string obd1FixNotFoundFixPromisedByDateTimeUTCString = "",
                    string absFixNotFoundFixPromisedByDateTimeUTCString = "",
                    string srsFixNotFoundFixPromisedByDateTimeUTCString = "",
                    string saveReport = "",
                    bool isUpdateNoFix = false,
                    string createdDateTimeUTCString = "",
                    string parentDiagnosticReportIdGuidString = "",
                    string manualRawFreezeFrameDataString = "",
                    bool additionalHelpRequired = false,
                    bool isNotifiedRequester = false,
                    DateTime? notifiedRequesterDateTimeUTC = null,
                    string notifiedRequesterVia = "",
                    string note = "")
        {

            bool IsObjectCreated = false;
            DiagReportInfo dr = new DiagReportInfo();
            WebServiceSessionStatus errors = new WebServiceSessionStatus();
            dr.WebServiceSessionStatus = errors;



            bool saveThisReport = (!string.IsNullOrEmpty(saveReport) && saveReport.ToLower() == "true");
            User user = GetOrCreateUserFromSystemUserIdGuidString(externalSystemUserIdGuidString, errors, saveThisReport);

            //if null, that means there were errors creating the user or signing the user up
            if (saveThisReport && user == null)
            {
                return dr;
            }


            if (user != null)
            {
                //update the user in the main Innova database
                if (!string.IsNullOrEmpty(externalSystemUserFirstName))
                {
                    user.FirstName = externalSystemUserFirstName;
                }
                if (!string.IsNullOrEmpty(externalSystemUserLastName))
                {
                    user.LastName = externalSystemUserLastName;
                }
                if (!string.IsNullOrEmpty(externalSystemUserEmailAddress))
                {
                    user.EmailAddress = externalSystemUserEmailAddress;
                }
                if (!string.IsNullOrEmpty(externalSystemUserPhoneNumber))
                {
                    user.PhoneNumber = externalSystemUserPhoneNumber;
                }
                if (!string.IsNullOrEmpty(externalSystemUserRegion))
                {
                    user.Region = externalSystemUserRegion.ToUpper(); //ie. CA
                }
                _getMostLikelyFixRepository.Save(user);
            }
            else
            {
                user.ExternalSystem = user.ExternalSystem;
            }



            /****************************************************************************
			* Create the diagnostic report
			* **************************************************************************/
            SoftwareType softwareType = SoftwareType.Unknown;
            ToolTypeFormat toolTypeFormat = ToolTypeFormat.NoneOrUnknown;
            DiagnosticReport diagnosticReport = new DiagnosticReport();

            if (!String.IsNullOrEmpty(diagnosticReportIdGuidString))
            {
                try
                {

                     diagnosticReport = _getMostLikelyFixRepository.GetDiagnosticReportByReportId(Convert.ToInt32(diagnosticReportIdGuidString));
                    _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);
                }
                catch (Exception ex)
                {
                    var error = ex;
                    errors.AddValidationFailure("20000", "The diagnostic report ID supplied does not exist in the system.");
                    return dr;
                }

                if (diagnosticReport.UserId != user.UserId)
                {
                    errors.AddValidationFailure("20001", "The user (" + user.UserId + ") that the existing diagnostic report was created for (" + diagnosticReport.UserId + ") does not match the supplied user");
                }

                if (isUpdateNoFix)
                {

                    //since this is an update
                    if (diagnosticReport.NoFixProcessCompletedAndSentDateTimeUTC == null)
                    {
                        errors.AddValidationFailure("20003", "The diagnostic report does not need to be updated any longer and was sent on " + diagnosticReport.NoFixProcessCompletedAndSentDateTimeUTC.ToString());
                        return dr;
                    }
                }
            }
            else
            {

                if (saveThisReport && isUpdateNoFix)
                {
                    errors.AddValidationFailure("80000", "You must supply the diagnostic report ID to update the report for no fixes");
                    return dr;
                }
                try
                {
                    softwareType = (SoftwareType)softwareTypeInt;
                }
                catch
                {
                    errors.AddValidationFailure("30000", "Unable to parse software type string");
                }

                try
                {
                    toolTypeFormat = (ToolTypeFormat)toolTypeFormatInt;
                }
                catch
                {
                    errors.AddValidationFailure("10000", "Unable to parse the tool type format string");
                }
            }


            if (errors.ValidationFailures.Length > 0)
            {
                return dr;
            }


            if (transmission.Trim().ToLower() != "automatic" && transmission.Trim().ToLower() != "standard")
            {
                if (saveThisReport)
                {
                    errors.AddValidationFailure("520", "The transmission control type must be \"automatic\" or \"standard\"");
                }
                else
                {
                    transmission = "automatic";
                }
            }

            if (transmission.Trim().ToLower() == "standard")
            {
                transmission = "Standard";
            }
            else
            {
                transmission = "Automatic";
            }



            //if not valid return here
            if (errors.ValidationFailures.Length > 0)
            {
                return dr;
            }

            //            /*****************************************************************************
            //			* Vehicle processing
            //			* **************************************************************************
            PolkVehicleYmme polkVehicleYMME = new PolkVehicleYmme();
            VehicleInfo v = new VehicleInfo();


            //Always decode the VIN if this is a US market, otherwise only decode if we got a VIN but don't have YMME values.
            if ((!String.IsNullOrEmpty(vin) && String.IsNullOrEmpty(manufacturer) && String.IsNullOrEmpty(year) && String.IsNullOrEmpty(make) && String.IsNullOrEmpty(model) && String.IsNullOrEmpty(engineType)))
            {

                if (errors.ValidationFailures.Length == 0)
                {
                    string errMessage = string.Empty;
                    try
                    {
                        //Added on 2017-07-30 11:50 PM by INNOVA Dev Team to check for the requested VIN is a masked VIN or not
                        polkVehicleYMME = _diagnosticReportService.DecodeVIN(vin, false);
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.ToString();
                    }

                    //Added on 2017-06-23 8:32 AM by INNOVA Dev Team: Throw exeption - error message is it is not null.
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
                }


                //populate the vehicle info object
                if (GlobalModel.UsePolkData == true)
                {
                    manufacturer = polkVehicleYMME.Manufacturer;
                    make = polkVehicleYMME.Make;
                    year = polkVehicleYMME.Year.ToString();
                    v.Model = polkVehicleYMME.Model;
                    model = polkVehicleYMME.Transmission;
                    trimLevel = polkVehicleYMME.EngineType;
                    engineType = polkVehicleYMME.Trim;
                    engineCode = polkVehicleYMME.EngineVinCode;
                    aaia = polkVehicleYMME.AAIA;
                }
            }
            else
            {
                v.IsValid = true;
                v.VIN = vin;
                v.ManufacturerName = manufacturer;
                v.Make = make;
                v.Year = year;
                v.Model = model;
                v.Transmission = transmission;
                v.EngineType = engineType;
                v.TrimLevel = trimLevel;
                v.EngineVINCode = engineCode;
                v.AAIA = aaia;

            }

            Vehicle vehicle = new Vehicle();

            if (user != null)
            {
                if (!String.IsNullOrEmpty(vin))
                {
                    vehicle = _getMostLikelyFixRepository.GetVehicleInfoByVinAndUserId(vin, user.UserId);
                }
                else
                {
                    int vehYear = 0;
                    int.TryParse(year, out vehYear);
                    vehicle = _getMostLikelyFixRepository.GetVehicleInfoForVehicleByYearMakeModelAsync(make, model, vehYear, engineType);
                }
            }


            //if vehicle is null
            if (vehicle == null)
            {
                //create a new vehicle here
                vehicle.VehicleId = vehicle.VehicleId;
                vehicle.UserId = vehicle.UserId;
                vehicle.VehicleTypeId = vehicle.VehicleTypeId;
                vehicle.ScheduleMaintenancePlanId = vehicle.ScheduleMaintenancePlanId;
                vehicle.ManufacturerNameAlt = vehicle.ManufacturerNameAlt;
                vehicle.Year = vehicle.Year;
                vehicle.Make = vehicle.Make;
                vehicle.Model = vehicle.Model;
                vehicle.EngineType = vehicle.EngineType;
                vehicle.EngineVINCode = vehicle.EngineVINCode;
                vehicle.TrimLevel = vehicle.TrimLevel;
                vehicle.TransmissionControlType = vehicle.TransmissionControlType;
                vehicle.FuelType = vehicle.FuelType;
                vehicle.VinStatus = vehicle.VinStatus;
                vehicle.PolkVehicleYMMEId = vehicle.PolkVehicleYMMEId;
                vehicle.Mileage = vehicle.Mileage;
                vehicle.MileageLastRecordedDateTimeUTC = vehicle.MileageLastRecordedDateTimeUTC;
                vehicle.IsActive = vehicle.IsActive;
                vehicle.SendNewTSBAlerts = vehicle.SendNewTSBAlerts;
                vehicle.NewTSBAlertLastSentDateTimeUTC = vehicle.NewTSBAlertLastSentDateTimeUTC;
                vehicle.User = user;
                vehicle.Vin = vin;
                vehicle.Nickname = "My " + year + " " + make + " " + model;
                vehicle.LicensePlateNumber = vehicle.LicensePlateNumber;
            }

            vehicle.PolkVehicleYmme = polkVehicleYMME;

            if (polkVehicleYMME != null && true)
            {
                vehicle.PolkVehicleYmme = polkVehicleYMME;
            }
            else
            {
                vehicle.Vin = vin;
                vehicle.ManufacturerNameAlt = manufacturer;
            }

            if (mileage >= 0)
            {
                vehicle.Mileage = mileage;
                vehicle.MileageLastRecordedDateTimeUTC = DateTime.UtcNow;
            }

            if (saveThisReport)
            {
                _getMostLikelyFixRepository.SaveChanges(vehicle);
            }

            if (diagnosticReport.IsPwrObd1FixFeedbackRequired == false)
            {
                if (diagnosticReport.Vehicle.VehicleId != vehicle.VehicleId)
                {
                    errors.AddValidationFailure("20002", "The vehicle the original diagnostic report is for does not match the supplied diagnostic report");
                }

            }
            dr.DiagnosticReportId = diagnosticReport.DiagnosticReportId;
            //set the vehicle (will be equal)
            diagnosticReport.Vehicle = vehicle;
            //set the user (will be equal)
            diagnosticReport.User = vehicle.User;

            //set the mileage
            if (vehicle.Mileage == null)
            {
                diagnosticReport.VehicleMileage = vehicle.Mileage.Value;
            }
            else
            {
                diagnosticReport.VehicleMileage = 0;
            }

            Device manualDevice = null;
            manualDevice = _getMostLikelyFixRepository.GetManualDevice(vehicle.UserId);
            string userId = null;
            if (IsObjectCreated)
            {
                //if there is an upload then set the tool information to the object
                if (!String.IsNullOrEmpty(rawUpload))
                {
                    try
                    {
                        userId = vehicle.User.UserId;
                        SetPropertiesAndToolInformationFromRawUploadString(rawUpload, toolTypeFormat, vehicle.User, diagnosticReport);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        errors.AddValidationFailure("20004", "The payload could not be decoded and/or processed. Please check your settings and try again." + Environment.NewLine + Environment.NewLine + ex.ToString());
                    }


                    //apply the device to the diagnostic report
                    Device device = new Device();
                    string toolId = null;
                    string partnerId = _getMostLikelyFixRepository.GetPartnerIdbyUderId(diagnosticReport.UserId)?.PartnerID;
                    ToolInformation toolInformation = GetToolInformationAsync(diagnosticReport.RawUploadString, diagnosticReport.Market, partnerId);
                    if (user.UserTypeExternalId == "00000000-0000-0000-0000-000000000015")
                    {
                        toolId = Convert.ToString(AUTOZONE_TOOL_ID);
                    }
                    else if (toolInformation != null && !string.IsNullOrEmpty(toolInformation.ToolId))
                    {
                        toolId = toolInformation.ToolId;
                    }

                    if (saveThisReport && !string.IsNullOrEmpty(toolId))
                    {
                        device = _getMostLikelyFixRepository.GetDeviceByChipIdAndUserIdAndActive(toolId, user.UserId);
                    }

                    if (device == null)
                    {
                        if (!string.IsNullOrWhiteSpace(toolId))
                        {
                            device.ChipId = toolId.ToString();
                        }
                        else
                        {
                            device.ChipId = FLEET_TOOL_ID.ToString();
                        }
                        device.UserId = user.UserId;
                        device.IsPrimaryOwner = true;
                        device.IsActive = true;
                        device.CreatedDateTimeUTC = DateTime.UtcNow;
                        device.UpdatedDateTimeUTC = device.CreatedDateTimeUTC;

                        if (saveThisReport)
                        {
                            _getMostLikelyFixRepository.SaveDevice(device);
                        }
                    }

                    diagnosticReport.Device = device;

                }
                else
                {
                    if (!string.IsNullOrEmpty(rawFreezeFrameDataString))
                    {
                        diagnosticReport.RawFreezeFrameDataString = rawFreezeFrameDataString;
                    }

                    if (!string.IsNullOrEmpty(rawMonitorsDataString))
                    {
                        diagnosticReport.RawMonitorsDataString = rawMonitorsDataString;
                    }


                    if (!String.IsNullOrEmpty(pwrPrimaryDtc))
                    {
                        pwrStoredErrorCodeList.Add(pwrPrimaryDtc);
                    }

                    if (!String.IsNullOrEmpty(pwrStoredDtcCommaSeparatedList))
                    {
                        foreach (string c in pwrStoredDtcCommaSeparatedList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!String.IsNullOrEmpty(c))
                            {
                                string code = c.Trim();
                                if (!pwrStoredErrorCodeList.Contains(code))
                                {
                                    pwrStoredErrorCodeList.Add(code);
                                }
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(pwrPendingDtcCommaSeparatedList))
                    {
                        //pending code list
                        foreach (string c in pwrPendingDtcCommaSeparatedList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!String.IsNullOrEmpty(c))
                            {
                                string code = c.Trim();
                                if (!pwrPendingErrorCodeList.Contains(code))
                                {
                                    pwrPendingErrorCodeList.Add(code);
                                }
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(pwrPermanentDtcCommaSeparatedList))
                    {
                        //pending code list
                        foreach (string c in pwrPermanentDtcCommaSeparatedList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!String.IsNullOrEmpty(c))
                            {
                                string code = c.Trim();
                                if (!pwrPermanentErrorCodeList.Contains(code))
                                {
                                    pwrPermanentErrorCodeList.Add(code);
                                }
                            }
                        }
                    }


                    //setup software type to be none, since this is a manual report
                    diagnosticReport.SoftwareType = (int)SoftwareType.Unknown;
                    //setup the tool type format to be none, since this is a manual report
                    diagnosticReport.ToolTypeFormat = (int)ToolTypeFormat.NoneOrUnknown;

                    //        //PowerTrain here
                    diagnosticReport.PwrMilCode = pwrPrimaryDtc;
                    diagnosticReport.PwrPendingCodesString = string.Join(",", pwrPendingErrorCodeList);
                    diagnosticReport.PwrStoredCodesString = string.Join(",", pwrStoredErrorCodeList);
                    diagnosticReport.PwrPermanentCodesString = string.Join(",", pwrPermanentErrorCodeList);
                    diagnosticReport.Obd1StoredCodesString = GetCodesStringFromCommaDelimitedString(obd1StoredCodesCommaSeparatedList);
                    diagnosticReport.Obd1PendingCodesString = GetCodesStringFromCommaDelimitedString(obd1PendingCodesCommaSeparatedList);
                    diagnosticReport.AbsStoredCodesString = GetCodesStringFromCommaDelimitedString(absStoredCodesCommaSeparatedList);
                    diagnosticReport.AbsPendingCodesString = GetCodesStringFromCommaDelimitedString(absPendingCodesCommaSeparatedList);
                    diagnosticReport.SrsStoredCodesString = GetCodesStringFromCommaDelimitedString(srsStoredCodesCommaSeparatedList);
                    diagnosticReport.SrsPendingCodesString = GetCodesStringFromCommaDelimitedString(srsPendingCodesCommaSeparatedList);

                    string partnerId = _getMostLikelyFixRepository.GetPartnerIdbyUderId(diagnosticReport.UserId)?.PartnerID;
                    ToolInformation toolInformation = GetToolInformationAsync(diagnosticReport.RawUploadString, diagnosticReport.Market, partnerId);


                    if (!String.IsNullOrEmpty(diagnosticReport.PwrMilCode))
                    {
                        diagnosticReport.ToolLEDStatus = (int)ToolLEDStatus.Red;
                        diagnosticReport.ToolMilStatus = (int)ToolMilStatus.On;
                    }

                    else if (PwrAllCodes(diagnosticReport).Count > 0 && (toolInformation.FreezeFrames == null || toolInformation.FreezeFrames.Count == 0))
                    {
                        bool allMMonitorsComplete = true;

                        foreach (Monitor m in toolInformation.Monitors)
                        {
                            if (String.IsNullOrEmpty(m.Value) || (!String.IsNullOrEmpty(m.Value) && m.Value.ToLower() != "complete"))
                            {
                                allMMonitorsComplete = false;
                                break;
                            }
                        }

                        // Test to see if all monitors are complete
                        if (allMMonitorsComplete)
                        {
                            diagnosticReport.ToolLEDStatus = (int)ToolLEDStatus.Green;
                        }
                        else
                        {
                            diagnosticReport.ToolLEDStatus = (int)ToolLEDStatus.Yellow;
                        }
                    }

                    else if (PwrAllCodes(diagnosticReport).Count > 0)
                    {
                        diagnosticReport.ToolLEDStatus = (int)ToolLEDStatus.Yellow;
                    }

                    diagnosticReport.IsManualReport = true;

                    if (saveThisReport && user != null)
                    {
                        diagnosticReport.Device = manualDevice;
                    }
                }
            }

            if (symptomRequest != null)
            {
                diagnosticReport.SymptomId = diagnosticReport.Symptom.SymptomId;
                diagnosticReport.PwrDiagnosticReportFixStatusWhenCreated = (int)DiagnosticReportFixStatus.FixNotFound;
                diagnosticReport.PwrDiagnosticReportFixStatus = (int)DiagnosticReportFixStatus.FixNotFound;
            }


            if ((diagnosticReport.Symptom != null && user != null) || (saveThisReport && user != null))
            {
                diagnosticReport.Device = manualDevice;
            }
            var needsErrorsCodes = _getMostLikelyFixRepository.GetDiagnosticReportResultErrorCodeCount(diagnosticReport.DiagnosticReportResultId);
            if (isUpdateNoFix)
            {
                bool save = false;
                if (diagnosticReport.PwrDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType.PowertrainObd2, false);
                    if (diagnosticReport.PwrDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixFound)
                    {
                        save = true;
                    }

                }
                if (diagnosticReport.Obd1DiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType.PowertrainOBD1, false);
                    if (diagnosticReport.Obd1DiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixFound)
                    {
                        save = true;
                    }
                }
                if (diagnosticReport.AbsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType.ABS, false);
                    if (diagnosticReport.AbsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixFound)
                    {
                        save = true;
                    }
                }
                if (diagnosticReport.SrsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType.SRS, false);
                    if (diagnosticReport.SrsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixFound)
                    {
                        save = true;
                    }
                }

                //if we just refreshed the report in one of the systems, then save it...
                if (save)
                {
                    if (!string.IsNullOrEmpty(createdDateTimeUTCString) && diagnosticReport.DiagnosticReportId != 0)
                    {
                        DateTime createdDateTimeUTC = DateTime.MinValue;
                        DateTime.TryParse(createdDateTimeUTCString, out createdDateTimeUTC);

                        if (createdDateTimeUTC != DateTime.MinValue)
                        {
                            diagnosticReport.CreatedDateTimeUTC = DateTime.Parse(createdDateTimeUTCString);
                            diagnosticReport.UpdatedDateTimeUTC = diagnosticReport.CreatedDateTimeUTC;
                        }
                    }
                    else
                    {
                        diagnosticReport.UpdatedDateTimeUTC = DateTime.UtcNow;
                    }
                    //-------------------------------------------------------------------------------------------------
                     diagnosticReport.UpdatedDateTimeUTC = DateTime.UtcNow;
                    _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);

                }
            }
            else
            {
                CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType.PowertrainObd2, false);
            }

            //if this is a new report, then let's set the dates that things are supposed to be sent to the customer by.
            if (IsObjectCreated)
            {

                DtcCodeViewCollection Obd1StoredCodes = GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.Obd1StoredCodesString);
                DtcCodeViewCollection AbsStoredCodes = GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.AbsStoredCodesString);
                DtcCodeViewCollection SrsStoredCodes = GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.SrsStoredCodesString);
                if ((!string.IsNullOrEmpty(diagnosticReport.PwrMilCode) || diagnosticReport.Symptom != null) && diagnosticReport.PwrDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    diagnosticReport.PwrFixNotFoundFixPromisedByDateTimeUTC = DateTime.Parse(pwrFixNotFoundFixPromisedByDateTimeUTCString);
                }
                if ((Obd1StoredCodes.Count > 0 || diagnosticReport.Symptom != null) && diagnosticReport.Obd1DiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    diagnosticReport.Obd1FixNotFoundFixPromisedByDateTimeUTC = DateTime.Parse(obd1FixNotFoundFixPromisedByDateTimeUTCString);
                }
                if ((AbsStoredCodes.Count > 0 || diagnosticReport.Symptom != null) && diagnosticReport.AbsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    diagnosticReport.AbsFixNotFoundFixPromisedByDateTimeUTC = DateTime.Parse(absFixNotFoundFixPromisedByDateTimeUTCString);
                }
                if ((SrsStoredCodes.Count > 0 || diagnosticReport.Symptom != null) && diagnosticReport.SrsDiagnosticReportFixStatus == (int)DiagnosticReportFixStatus.FixNotFound)
                {
                    diagnosticReport.SrsFixNotFoundFixPromisedByDateTimeUTC = DateTime.Parse(srsFixNotFoundFixPromisedByDateTimeUTCString);
                }

                if (!string.IsNullOrWhiteSpace(parentDiagnosticReportIdGuidString))
                diagnosticReport.ManualRawFreezeFrameDataString = manualRawFreezeFrameDataString;
                diagnosticReport.AdditionalHelpRequired = additionalHelpRequired;
                diagnosticReport.IsNotifiedRequester = isNotifiedRequester;
                diagnosticReport.NotifiedRequesterDateTimeUTC = notifiedRequesterDateTimeUTC;
                diagnosticReport.NotifiedRequesterVia = notifiedRequesterVia;
                diagnosticReport.Note = note;
            }


           // if not updating the fix and the user has sent in
            if (saveThisReport)
            {
                //-----------------------------------------Moved from LogDiagnosticReportWithMileage() - DiagnosticReportLogging.asmx-----------------------
                // Updated the report's created date/time if we got a value.
                if (!string.IsNullOrEmpty(createdDateTimeUTCString) && diagnosticReport.DiagnosticReportId != 0)
                {
                    DateTime createdDateTimeUTC = DateTime.MinValue;
                    DateTime.TryParse(createdDateTimeUTCString, out createdDateTimeUTC);

                    if (createdDateTimeUTC != DateTime.MinValue)
                    {
                        diagnosticReport.CreatedDateTimeUTC = DateTime.Parse(createdDateTimeUTCString);
                        diagnosticReport.UpdatedDateTimeUTC = diagnosticReport.CreatedDateTimeUTC;
                    }
                }
                int DaysMasterTechHaveToProvideAFix = GlobalModel.DaysMasterTechHaveToProvideAFix;
                bool MasterTechAssignPwrNoFixReports = GlobalModel.MasterTechAssignPwrNoFixReports;
                bool MasterTechAssignObd1NoFixReports = GlobalModel.MasterTechAssignObd1NoFixReports;
                bool MasterTechAssignAbsNoFixReports = GlobalModel.MasterTechAssignAbsNoFixReports;
                bool MasterTechAssignSrsNoFixReports = GlobalModel.MasterTechAssignSrsNoFixReports;
        



                if (isUpdateNoFix)
                {
                    //save the diagnostic report
                    if (string.IsNullOrEmpty(createdDateTimeUTCString))
                    {
                        diagnosticReport.UpdatedDateTimeUTC = DateTime.UtcNow;
                    }

                    diagnosticReport.UpdatedDateTimeUTC = DateTime.UtcNow;
                    _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);
                    AssignNoFixReportToMasterTechAndSave(diagnosticReport, vehicle, DaysMasterTechHaveToProvideAFix, MasterTechAssignPwrNoFixReports, MasterTechAssignObd1NoFixReports, MasterTechAssignAbsNoFixReports, MasterTechAssignSrsNoFixReports);

                }
                else if (needsErrorsCodes.Count > 0)
                {
                    _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);
                    AssignNoFixReportToMasterTechAndSave(diagnosticReport, vehicle, DaysMasterTechHaveToProvideAFix, MasterTechAssignPwrNoFixReports, MasterTechAssignObd1NoFixReports, MasterTechAssignAbsNoFixReports, MasterTechAssignSrsNoFixReports);

                }
            }
            //-------------------------------------------------------------------------------------------------
            /*********************************************************************************************************
            * Main processing is complete, now prepare the data and output it to the Web Service Object!
            * *******************************************************************************************************/

                dr.Vehicle = v;
                int diagnosticReportId = (int)dr.DiagnosticReportId;
                return GetDiagnosticReportExisting(diagnosticReportId, ToBoolean(includeRecallsForVehicle), ToBoolean(includeTSBCountForVehicle), ToBoolean(includeTSBsForVehicleAndMatchingErrorCodes), ToBoolean(includeNextScheduledMaintenance), ToBoolean(includeWarrantyInfo), true);

        }


        public void AssignNoFixReportToMasterTechAndSave(DiagnosticReport diagnosticReport, Vehicle vehicle, int maxDaysToProvideFeedback, bool masterTechAssignPwrNoFixReports, bool masterTechAssignObd1NoFixReports, bool masterTechAssignAbsNoFixReports, bool masterTechAssignSrsNoFixReports)
        {
            List<User> masterTechUsers = _getMostLikelyFixRepository.GetOBDFixMasterTechs(vehicle.Make, vehicle.UserId);
            AssignNoFixReportToMasterTechAndSave(diagnosticReport, vehicle, masterTechUsers, maxDaysToProvideFeedback, masterTechAssignPwrNoFixReports, masterTechAssignObd1NoFixReports, masterTechAssignAbsNoFixReports, masterTechAssignSrsNoFixReports);
        }

        public void AssignNoFixReportToMasterTechAndSave(DiagnosticReport diagnosticReport,Vehicle vehicle, List<User> masterTechs, int maxDaysToProvideFeedback, bool masterTechAssignPwrNoFixReports, bool masterTechAssignObd1NoFixReports, bool masterTechAssignAbsNoFixReports, bool masterTechAssignSrsNoFixReports)
        {
            DateTime minCreatedDateTimeUTC = DateTime.Now.Date.AddDays(0 - maxDaysToProvideFeedback).ToUniversalTime();

            if (diagnosticReport.HasMasterTechsAssigned == false
                    &&
                    masterTechs != null
                    &&
                    (
                        diagnosticReport.CreatedDateTimeUTC >= minCreatedDateTimeUTC
                        ||
                        diagnosticReport.MasterTechProvideFixFeedbackByOverrideDateTimeUTC >= DateTime.UtcNow
                    )
                    &&
                    (
                        (masterTechAssignPwrNoFixReports && this.PwrDiagnosticReportFixStatus == DiagnosticReportFixStatus.FixNotFound && diagnosticReport.PwrFixNotFoundFixPromisedByDateTimeUTC.HasValue)
                        ||
                        (masterTechAssignObd1NoFixReports && this.Obd1DiagnosticReportFixStatus == DiagnosticReportFixStatus.FixNotFound && diagnosticReport.Obd1FixNotFoundFixPromisedByDateTimeUTC.HasValue)
                        ||
                        (masterTechAssignAbsNoFixReports && this.AbsDiagnosticReportFixStatus == DiagnosticReportFixStatus.FixNotFound && diagnosticReport.AbsFixNotFoundFixPromisedByDateTimeUTC.HasValue)
                        ||
                        (masterTechAssignSrsNoFixReports && this.SrsDiagnosticReportFixStatus == DiagnosticReportFixStatus.FixNotFound && diagnosticReport.SrsFixNotFoundFixPromisedByDateTimeUTC.HasValue)
                    )
                )
            {

                foreach (User mt in masterTechs)
                {
                    if (MasterTechMakesLowerCase.Contains(vehicle.Make.ToLower()))
                    {
                        this.AddAssignedMasterTech(mt, diagnosticReport);
                         mt.MasterTechNoFixReportLastAssignedDateTimeUTC = DateTime.UtcNow;
                        _getMostLikelyFixRepository.SaveUser(mt);
                    }
                }
              _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);
            }
        }


        public void AddAssignedMasterTech(User masterTech,DiagnosticReport diagnosticReport)
        {
            if (this.MasterTechsAssigned.Where(x => x.UserId == masterTech.UserId) == null)
            {
                this.MasterTechsAssigned.Add(masterTech);
                diagnosticReport.HasMasterTechsAssigned = true;
                isMasterTechsAssignedDirty = true;
                IsObjectDirty = true;
            }
        }
        public List<string> MasterTechMakesLowerCase
        {
            get
            {
                List<string> masterTechMakesLowerCase = new List<string>();

                foreach (string s in this.MasterTechMakes)
                {
                    masterTechMakesLowerCase.Add(s.ToLower());
                }

                return masterTechMakesLowerCase;
            }
        }
        public List<string> MasterTechMakes
        {
            get
            {
                if (this.masterTechMakes == null)
                {
                    this.masterTechMakes = new List<string>();

                    if (!this.isObjectCreated)
                    {
                        if (this.masterTechMakesString != "")
                        {
                            foreach (string s in this.masterTechMakesString.Split("|".ToCharArray()))
                            {
                                if (s != null && s != "" && !this.masterTechMakes.Contains(s))
                                {
                                    this.masterTechMakes.Add(s);
                                }
                            }
                        }
                    }
                }
                return this.masterTechMakes;
            }
        }
        private string GetCodesStringFromCommaDelimitedString(string commaDelimitedCodes)
        {
            string stringValue = "";
            if (!String.IsNullOrEmpty(commaDelimitedCodes))
            {
                commaDelimitedCodes = commaDelimitedCodes.Replace(" ", "");
                stringValue = commaDelimitedCodes.Replace(",", ",Manual|") + ",Manual";
            }
            return stringValue;
        }

        private User GetOrCreateUserFromSystemUserIdGuidString(string externalSystemUserIdGuidString, WebServiceSessionStatus errors, bool createUserIfNotFound)
        {
            Guid userId = Guid.Empty;
            try
            {
                userId = new Guid(externalSystemUserIdGuidString);
            }
            catch
            {
                if (createUserIfNotFound)
                {
                    errors.AddValidationFailure("20006", "You must supply a valid GUID for the userID in string format");
                }
                return null;
            }

            User user = _getMostLikelyFixRepository.GetUser(userId).Result;

            if (user.ExternalSystem.ExternalSystemId == null)
            {
                errors.AddValidationFailure("20005", "The User ID specified with the parameter \"externalSystemUserIdGuidString\" was created by an External System that does not match the current external system key.");
                return null;
            }
            return user;
        }

        public void SetPropertiesAndToolInformationFromRawUploadString(string rawUploadString, ToolTypeFormat toolTypeFormat, User user,DiagnosticReport diagnosticReport)
        {
            ExternalSystem externalSystem = new ExternalSystem();
            externalSystem = _getMostLikelyFixRepository.GetPartnerIdbyUderId(user.UserId);
            string vin = _getMostLikelyFixRepository.GetVin(user.UserId);
            SetPropertiesFromToolInformation(GetToolInformationAsync(rawUploadString, 0, externalSystem.PartnerID), diagnosticReport);
            bool saveUpdatedRawUploadString = false;
            if (rawUploadString.StartsWith(FLEET_PAYLOAD_PREFIX))
            {
                rawUploadString = FLEET_PAYLOAD_PREFIX + rawUploadString;
                saveUpdatedRawUploadString = true;
            }

            diagnosticReport.RawUploadString = rawUploadString;

            if (saveUpdatedRawUploadString)
            {
                _getMostLikelyFixRepository.SaveDiagnosticReport(diagnosticReport);
            }
        }

        private void SetPropertiesFromToolInformation(ToolInformation _toolInformation,DiagnosticReport diagnosticReport)
        {

            ToolInformation toolInformation = new ToolInformation();
            toolInformation.ToolTypeFormat = _toolInformation.ToolTypeFormat;
            toolInformation.SoftwareType = _toolInformation.SoftwareType;


            if (toolInformation.SoftwareVersion != null)
            {

                diagnosticReport.SoftwareVersion = toolInformation.SoftwareVersion.ToString();
            }
            if (toolInformation.FirmwareVersion != null)
            {
                diagnosticReport.FirmwareVersion = toolInformation.FirmwareVersion.ToString();
            }

            //   this.VIN = toolInformation.Vin;
            string VIN = toolInformation.Vin;
            DtcCodeViewCollection Obd1StoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.Obd1StoredCodesString);
            foreach (var obd1 in toolInformation.AllObd1Codes)
            {
                DtcCodeView dcv = (DtcCodeView)Obd1StoredCodes.FindByProperty("CodeType", obd1.Description);
                bool isNew = false;

                if (dcv == null)
                {
                    dcv = new DtcCodeView(obd1.Description);
                    isNew = true;
                }

                if (!dcv.Codes.Contains(obd1.Value))
                {
                    dcv.Codes.Add(obd1.Value);
                }

                if (isNew)
                {
                    Obd1StoredCodes.Add(dcv);
                }
            }

            DtcCodeViewCollection AbsPendingCodes = this.GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.AbsPendingCodesString);
            foreach (Abs abs in toolInformation.PendingAbsCodes)
            {
                DtcCodeView dcv = (DtcCodeView)AbsPendingCodes.FindByProperty("CodeType", abs.Description);
                bool isNew = false;

                if (dcv == null)
                {
                    dcv = new DtcCodeView(abs.Description);
                    isNew = true;
                }

                if (!dcv.Codes.Contains(abs.Value))
                {
                    dcv.Codes.Add(abs.Value);
                }

                if (isNew)
                {
                    AbsPendingCodes.Add(dcv);
                }
            }

            DtcCodeViewCollection AbsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.AbsStoredCodesString);
            foreach (Abs abs in toolInformation.StoredAbsCodes)
            {
                DtcCodeView dcv = (DtcCodeView)AbsStoredCodes.FindByProperty("CodeType", abs.Description);
                bool isNew = false;

                if (dcv == null)
                {
                    dcv = new DtcCodeView(abs.Description);
                    isNew = true;
                }

                if (!dcv.Codes.Contains(abs.Value))
                {
                    dcv.Codes.Add(abs.Value);
                }

                if (isNew)
                {
                    AbsStoredCodes.Add(dcv);
                }
            }

            if (toolInformation.AllAbss != null)
            {
                foreach (Abs abs in toolInformation.AllAbss)
                {
                    DtcCodeView dcv = (DtcCodeView)AbsStoredCodes.FindByProperty("CodeType", string.Empty);
                    bool isNew = false;

                    if (dcv == null)
                    {
                        dcv = new DtcCodeView(string.Empty);
                        isNew = true;
                    }

                    if (!dcv.Codes.Contains(abs.Value))
                    {
                        dcv.Codes.Add(abs.Value);
                    }

                    if (isNew)
                    {
                        AbsStoredCodes.Add(dcv);
                    }
                }
            }

            DtcCodeViewCollection SrsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.SrsStoredCodesString);

            foreach (Srs srs in toolInformation.StoredSrsCodes)
            {
                DtcCodeView dcv = (DtcCodeView)SrsStoredCodes.FindByProperty("CodeType", srs.Description);
                bool isNew = false;

                if (dcv == null)
                {
                    dcv = new DtcCodeView(srs.Description);
                    isNew = true;
                }

                if (!dcv.Codes.Contains(srs.Value))
                {
                    dcv.Codes.Add(srs.Value);
                }

                if (isNew)
                {
                    SrsStoredCodes.Add(dcv);
                }
            }

            if (toolInformation.AllSrss != null)
            {
                foreach (Srs srs in toolInformation.AllSrss)
                {
                    DtcCodeView dcv = (DtcCodeView)SrsStoredCodes.FindByProperty("CodeType", string.Empty);
                    bool isNew = false;

                    if (dcv == null)
                    {
                        dcv = new DtcCodeView(string.Empty);
                        isNew = true;
                    }

                    if (!dcv.Codes.Contains(srs.Value))
                    {
                        dcv.Codes.Add(srs.Value);
                    }

                    if (isNew)
                    {
                        SrsStoredCodes.Add(dcv);
                    }
                }
            }

            DtcCodeViewCollection SrsPendingCodes = this.GetDtcCodeViewCollectionFromDelimitedString(diagnosticReport.SrsPendingCodesString);
            foreach (Srs srs in toolInformation.PendingSrsCodes)
            {
                DtcCodeView dcv = (DtcCodeView)SrsPendingCodes.FindByProperty("CodeType", srs.Description);
                bool isNew = false;

                if (dcv == null)
                {
                    dcv = new DtcCodeView(srs.Description);
                    isNew = true;
                }

                if (!dcv.Codes.Contains(srs.Value))
                {
                    dcv.Codes.Add(srs.Value);
                }

                if (isNew)
                {
                    SrsPendingCodes.Add(dcv);
                }
            }

            foreach (PowerTrain pt in toolInformation.PendingPowerTrains)
            {
                this.PwrPendingCodes.Add(pt.DTC);
            }

            foreach (PowerTrain pt in toolInformation.StoredPowerTrains)
            {
                this.PwrStoredCodes.Add(pt.DTC);
            }

            foreach (PowerTrain pt in toolInformation.PermanentPowerTrains)
            {
                this.PwrPermanentCodes.Add(pt.DTC);
            }

            diagnosticReport.PwrMilCode = toolInformation.PrimaryDtc;
            diagnosticReport.ToolLEDStatus = (int)toolInformation.ToolLEDStatus;
            diagnosticReport.ToolMilStatus = (int)toolInformation.ToolMilStatus;
        }

        public DtcCodeViewCollection GetDtcCodeViewCollectionFromDelimitedString(string delimitedDtcCategoryValues)
        {
            DtcCodeViewCollection dtcCodeViews = new DtcCodeViewCollection();

            if (!string.IsNullOrEmpty(delimitedDtcCategoryValues))
            {
                string[] codeCategoryDelimitedString = delimitedDtcCategoryValues.Split(new char[] { '|' });
                for (int i = 0; i < codeCategoryDelimitedString.Length; i++)
                {
                    string[] codeCategoryValues = codeCategoryDelimitedString[i].Split(new char[] { ',' });
                    string code = codeCategoryValues[0];
                    string category = codeCategoryValues[1];

                    DtcCodeView dtcView = (DtcCodeView)dtcCodeViews.FindByProperty("CodeType", category);

                    if (dtcView == null)
                    {
                        dtcView = new DtcCodeView(category);
                        dtcCodeViews.Add(dtcView);
                    }

                    dtcView.Codes.Add(code);
                }
            }
            return dtcCodeViews;
        }

        public ToolInformation GetToolInformationAsync(string rawUploadString, int? langways, string partnerID)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://52.91.10.67/api/InnovaServiceUtilitiesWrapper/DecoderWrapperForInnova");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "5d195afa-d0ea-4999-8196-f4152173e19f");
                    var myContent = JsonConvert.SerializeObject(new { RawUploadString = rawUploadString, CurrentLanguage = 0, PartnerId = partnerID });
                    var response = client.PostAsync("", new StringContent(myContent, Encoding.UTF8, "application/json")).Result;
                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ToolInformation>(responseResult);
                }
                catch (Exception)
                {
                    return new ToolInformation();
                }
            }
        }

        private List<string> PwrAllCodes(DiagnosticReport diagnosticReport)
        {
            List<string> codes = new List<string>();

            if (!string.IsNullOrEmpty(diagnosticReport.PwrMilCode))
            {
                codes.Add(diagnosticReport.PwrMilCode);
            }

            foreach (string code in this.PwrStoredCodes)
            {
                if (!string.IsNullOrEmpty(code) && !codes.Contains(code))
                {
                    codes.Add(code);
                }
            }
            foreach (string code in this.PwrPendingCodes)
            {
                if (!string.IsNullOrEmpty(code) && !codes.Contains(code))
                {
                    codes.Add(code);
                }
            }
            foreach (string code in this.PwrPermanentCodes)
            {
                if (!string.IsNullOrEmpty(code) && !codes.Contains(code))
                {
                    codes.Add(code);
                }
            }
            return codes;
        }

        public void CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType, bool updateErrorCodes)
        {
            CreateDiagnosticReportResult(diagnosticReportErrorCodeSystemType, updateErrorCodes, false, false);
        }

        public void CreateDiagnosticReportResult(DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType, bool updateErrorCodes, bool logDiscrepancies, bool onlyProcessFixes)
        {
            if (SymptomGuids.Any())
            {
                CreateDiagnosticReportResultFixForSymptoms();
                return;
            }

            bool fixFound = false;

            if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.PowertrainObd2)
            {
                fixFound = CreateDiagnosticReportResult(_diagnosticReport.PwrMilCode, DiagnosticReportErrorCodeSystemType.PowertrainObd2, updateErrorCodes, logDiscrepancies, onlyProcessFixes);

                if (!pwrFixFoundAfterLastFixLookup)
                {
                    pwrFixFoundAfterLastFixLookup = fixFound;
                }
            }
            else if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.PowertrainOBD1)
            {
                DtcCodeViewCollection Obd1StoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1StoredCodesString);
                foreach (string code in Obd1StoredCodes.AllDtcs)
                {
                    if (this.CreateDiagnosticReportResult(code, DiagnosticReportErrorCodeSystemType.PowertrainOBD1, updateErrorCodes, logDiscrepancies, onlyProcessFixes))
                    {
                        fixFound = true;
                    }
                }

                if (!obd1FixFoundAfterLastFixLookup)
                {
                    obd1FixFoundAfterLastFixLookup = fixFound;
                }
            }
            else if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.ABS)
            {
                DtcCodeViewCollection AbsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsStoredCodesString);
                foreach (string code in AbsStoredCodes)
                {
                    if (this.CreateDiagnosticReportResult(code, DiagnosticReportErrorCodeSystemType.ABS, updateErrorCodes, logDiscrepancies, onlyProcessFixes))
                    {
                        fixFound = true;
                    }
                }

                if (!this.absFixFoundAfterLastFixLookup)
                {
                    absFixFoundAfterLastFixLookup = fixFound;
                }
            }
            else if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.SRS)
            {
                DtcCodeViewCollection SrsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsStoredCodesString);
                foreach (string code in SrsStoredCodes)
                {
                    if (this.CreateDiagnosticReportResult(code, DiagnosticReportErrorCodeSystemType.SRS, updateErrorCodes, logDiscrepancies, onlyProcessFixes))
                    {
                        fixFound = true;
                    }
                }

                if (!srsFixFoundAfterLastFixLookup)
                {
                    srsFixFoundAfterLastFixLookup = fixFound;
                }
            }
            else if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.Enhanced && updateErrorCodes)
            {
                if (this.diagnosticReportResult == null)
                {
                    _getMostLikelyFixRepository.SaveDiagnosticReportResult(_diagnosticReport.DiagnosticReportId);
                }
            }
            if (fixFound && this.FixProvidedDateTimeUTC == null)
            {
                this.FixProvidedDateTimeUTC = DateTime.UtcNow;
            }
        }

        private bool CreateDiagnosticReportResult(string primaryDtc, DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType, bool updateErrorCodes, bool logDiscrepancies, bool onlyProcessFixes)
        {
            bool newFixFound = false;
            DiagnosticReportFixStatus oldFixStatus = DiagnosticReportFixStatus.FixNotFound;

            if (diagnosticReportErrorCodeSystemType != DiagnosticReportErrorCodeSystemType.Enhanced)
            {
                if (string.IsNullOrEmpty(primaryDtc))
                {
                    return false;
                }

                List<Fix> fixes = new List<Fix>();

                if (diagnosticReportResult == null)
                {
                    _getMostLikelyFixRepository.SaveDiagnosticReportResult(_diagnosticReport.DiagnosticReportId);
                }

                switch (diagnosticReportErrorCodeSystemType)
                {
                    case DiagnosticReportErrorCodeSystemType.ABS:
                        fixes =GetFixesSorted(primaryDtc, null, logDiscrepancies, onlyProcessFixes, diagnosticReportErrorCodeSystemType);
                        oldFixStatus = this.AbsDiagnosticReportFixStatus;
                        break;

                    case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                        fixes = this.GetFixesSorted(primaryDtc, null, logDiscrepancies, onlyProcessFixes, diagnosticReportErrorCodeSystemType);
                        oldFixStatus = this.Obd1DiagnosticReportFixStatus;
                        break;

                    case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                        fixes = GetFixesSorted(primaryDtc, null, logDiscrepancies, onlyProcessFixes, diagnosticReportErrorCodeSystemType);
                        oldFixStatus = this.PwrDiagnosticReportFixStatus;
                        break;

                    case DiagnosticReportErrorCodeSystemType.SRS:
                        fixes = this.GetFixesSorted(primaryDtc, null, logDiscrepancies, onlyProcessFixes, diagnosticReportErrorCodeSystemType);
                        oldFixStatus = this.SrsDiagnosticReportFixStatus;
                        break;
                }

                if (!onlyProcessFixes)
                {
                    //this.DiagnosticReportResult.DiagnosticReportResultFixes.LoadRelation(this.DiagnosticReportResult.DiagnosticReportResultFixes.RelationDiagnosticReportResultFixeParts);
                    ArrayList oldFixes = null;
                    foreach (DiagnosticReportResultFix f in diagnosticReportResultFixes)
                    {
                        if (f.DiagnosticReportErrorCodeSystemType == (int)diagnosticReportErrorCodeSystemType)
                        {
                            if (String.Equals(f.PrimaryErrorCode, primaryDtc, StringComparison.OrdinalIgnoreCase))
                            {
                                if (oldFixes == null)
                                {
                                    oldFixes = new ArrayList();
                                }
                                oldFixes.Add(f);
                            }
                        }
                    }

                    if (oldFixes != null && oldFixes.Count > 0)
                    {
                        foreach (DiagnosticReportResultFix drrFix in oldFixes)
                        {
                            diagnosticReportResultFixes.Remove(drrFix);
                        }
                    }
                    for (int i = 0; i < fixes.Count; i++)
                    {
                        Fix fix = fixes[i];
                        DiagnosticReportResultFix drrFix = SetDiagnosticReportResultFix(fix);
                        drrFix.PrimaryErrorCode = primaryDtc;
                        drrFix.DiagnosticReportErrorCodeSystemType = (int)diagnosticReportErrorCodeSystemType;
                        drrFix.SortOrder = i;
                        diagnosticReportResultFixes.Add(drrFix);
                    }
                }
            }

            if (!onlyProcessFixes)
            {
                this.SetFixStatuses(diagnosticReportErrorCodeSystemType);

                DiagnosticReportFixStatus newFixStatus = this.GetFixStatus(diagnosticReportErrorCodeSystemType);

                if (newFixStatus == DiagnosticReportFixStatus.FixFound && newFixStatus != oldFixStatus)
                {
                    newFixFound = true;
                }

                if (!onlyProcessFixes && updateErrorCodes)
                {
                    this.CreateDiagnosticReportResultErrorCodes(diagnosticReportErrorCodeSystemType);
                }
            }
            return newFixFound;
        }

        private void CreateDiagnosticReportResultErrorCodes(DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType)
        {
            if (this.diagnosticReportResult == null)
            {
                _getMostLikelyFixRepository.SaveDiagnosticReportResult(_diagnosticReport.DiagnosticReportId);
            }

            List<string> allCodes = new List<string>();
            List<string> storedCodes = new List<string>();
            List<string> pendingCodes = new List<string>();
            List<string> permanentCodes = new List<string>(); //Added on November 23 2016 3:55PM to support the new Code Type

            DtcCodeViewCollection Obd1StoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1StoredCodesString);
            DtcCodeViewCollection AbsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsStoredCodesString);
            DtcCodeViewCollection SrsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsStoredCodesString);
            DtcCodeViewCollection AbsPendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsPendingCodesString);
            DtcCodeViewCollection Obd1PendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1PendingCodesString);
            DtcCodeViewCollection SrsPendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsPendingCodesString);

            switch (diagnosticReportErrorCodeSystemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    allCodes = this.AbsAllCodes;
                    storedCodes = AbsStoredCodes.AllDtcs;
                    pendingCodes = AbsPendingCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    allCodes = this.Obd1AllCodes;
                    storedCodes = Obd1StoredCodes.AllDtcs;
                    pendingCodes = Obd1PendingCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    allCodes = PwrAllCodes(_diagnosticReport);
                    storedCodes = this.PwrStoredCodes;
                    pendingCodes = this.PwrPendingCodes;
                    permanentCodes = this.PwrPermanentCodes; //Added on November 23 2016 3:57PM to support the new Code Type
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    allCodes = this.SrsAllCodes;
                    storedCodes = SrsStoredCodes.AllDtcs;
                    pendingCodes = SrsPendingCodes.AllDtcs;
                    break;
            }

            if (allCodes.Count == 0)
            {
                return;
            }

            bool foundPrimary = false;
            bool foundFirstStored = false;
            bool foundFirstPending = false;
            bool foundFirstPermanent = false;

            List<string> tempStoredErrorCodes = new List<string>();
            List<string> tempPendingErrorCodes = new List<string>();
            List<string> tempPermanentErrorCodes = new List<string>();


            //get the dtc codes
            List<DTCCode> dtcCodes = this.GetDtcCodes(allCodes);
            //get the master codes
            List<DTCMasterCodeList> dtcMasterCodes = this.GetDtcMasterCodes(allCodes);
            //and get the delmar codes
            List<VehicleTypeCodeAssignment> codeAssignments = null;
            List<VehicleType> vehicleTypes = this.GetDelmarVehicleTypes();
            if (vehicleTypes.Count > 0)
            {
                codeAssignments = this.GetDelmarVehicleCodeAssignments(vehicleTypes, allCodes);
            }

            List<DiagnosticReportResultErrorCode> oldDrrErrorCodes = _getMostLikelyFixRepository.GetOldDiagnosticErrorCode(_diagnosticReport.DiagnosticReportId);
            if (oldDrrErrorCodes != null)
            {
                foreach (DiagnosticReportResultErrorCode oldDrrErrorCode in oldDrrErrorCodes)
                {
                    diagnosticReportResultErrorCode.Remove(oldDrrErrorCode);
                }
            }

            for (int i = 0; i < allCodes.Count; i++)
            {
                string errorCode = allCodes[i];

                DiagnosticReportErrorCodeType diagnosticReportErrorCodeType;

                //determine the type of error code from the list
                if (diagnosticReportErrorCodeSystemType == DiagnosticReportErrorCodeSystemType.PowertrainObd2 &&
                    !foundPrimary && errorCode == _diagnosticReport.PwrMilCode
                    && _diagnosticReport.ToolLEDStatus == (int)ToolLEDStatus.Red)
                {
                    tempStoredErrorCodes.Add(errorCode);

                    foundPrimary = true;
                    diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.PrimaryDiagnosticReportErrorCode;
                    DiagnosticReportResultErrorCode drrErrorCode = this.GetDiagnosticReportResultErrorCode(errorCode, diagnosticReportErrorCodeType, "", dtcCodes, codeAssignments, dtcMasterCodes);

                    if (drrErrorCode != null)
                    {
                        drrErrorCode.DiagnosticReportErrorCodeSystemType = (int)diagnosticReportErrorCodeSystemType;
                        drrErrorCode.SortOrder = i;
                        //add the error code to the list for this report result
                        diagnosticReportResultErrorCode.Add(drrErrorCode);
                    }
                }

                //if the code is in the stored code list and not in the processed list then
                if (storedCodes.Contains(errorCode) && !tempStoredErrorCodes.Contains(errorCode))
                {
                    tempStoredErrorCodes.Add(errorCode);

                    if (!foundFirstStored)
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.FirstStoredDiagnosticReportErrorCode;
                        foundFirstStored = true;
                    }
                    else
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.AdditionalStoredDiagnosticReportErrorCode;
                    }

                    DiagnosticReportResultErrorCode drrErrorCode = this.GetDiagnosticReportResultErrorCode(errorCode, diagnosticReportErrorCodeType, "", dtcCodes, codeAssignments, dtcMasterCodes);
                    if (drrErrorCode != null)
                    {
                        drrErrorCode.DiagnosticReportErrorCodeSystemType = (int)diagnosticReportErrorCodeSystemType;
                        drrErrorCode.SortOrder = i;
                        //add the error code to the list for this report result
                        diagnosticReportResultErrorCode.Add(drrErrorCode);
                    }
                }

                if (pendingCodes.Contains(errorCode) && !tempPendingErrorCodes.Contains(errorCode))
                {
                    if (!foundFirstPending)
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.FirstPendingDiagnosticReportErrorCode;
                        foundFirstPending = true;
                    }
                    else
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.AdditionalPendingDiagnosticReportErrorCode;
                    }

                    DiagnosticReportResultErrorCode drrErrorCode = this.GetDiagnosticReportResultErrorCode(errorCode, diagnosticReportErrorCodeType, "", dtcCodes, codeAssignments, dtcMasterCodes);
                    if (drrErrorCode != null)
                    {
                        drrErrorCode.DiagnosticReportErrorCodeSystemType = (int)diagnosticReportErrorCodeSystemType;
                        drrErrorCode.SortOrder = i;
                        //add the error code to the list for this report result
                        diagnosticReportResultErrorCode.Add(drrErrorCode);
                    }
                }

                //Added on November 23 2016 4:05PM to support the Permanent Code Type.
                if (permanentCodes.Contains(errorCode) && !tempPermanentErrorCodes.Contains(errorCode))
                {
                    if (!foundFirstPermanent)
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.FirstPermanentDiagnosticReportErrorCode;
                        foundFirstPermanent = true;
                    }
                    else
                    {
                        diagnosticReportErrorCodeType = DiagnosticReportErrorCodeType.AdditionalPermanentDiagnosticReportErrorCode;
                    }

                    DiagnosticReportResultErrorCode drrErrorCode = this.GetDiagnosticReportResultErrorCode(errorCode, diagnosticReportErrorCodeType, "", dtcCodes, codeAssignments, dtcMasterCodes);
                    if (drrErrorCode != null)
                    {
                        drrErrorCode.DiagnosticReportErrorCodeSystemType = (int)diagnosticReportErrorCodeSystemType;
                        drrErrorCode.SortOrder = i;
                        diagnosticReportResultErrorCode.Add(drrErrorCode);
                    }
                }
            }
        }


        private DiagnosticReportResultErrorCode GetDiagnosticReportResultErrorCode(string errorCode, DiagnosticReportErrorCodeType diagnosticReportErrorCodeType, string errorCodeType, List<DTCCode> dtcCodes, List<VehicleTypeCodeAssignment> vehicleCodeAssignments, List<DTCMasterCodeList> dtcMasterCodes)
        {
            DiagnosticReportResultErrorCode drrErrorCode = new DiagnosticReportResultErrorCode();

            drrErrorCode.DiagnosticReportResultId = diagnosticReportResult.DiagnosticReportResultId;

            bool foundErrorCodes = false;
            DTCCode dtcCode = null;

            foreach (DTCCode c in dtcCodes)
            {
                if (String.Equals(errorCode, c.ErrorCode, StringComparison.OrdinalIgnoreCase))
                {
                    dtcCode = c;
                    break;
                }
            }

            //if we found an innova specific code then we add the diagnostic report for it.
            if (dtcCode != null)
            {
                //add the master item to the list
                AddDiagnosticReportResultErrorCodeDefinition(dtcCode);
            }
            else
            {
                if (vehicleCodeAssignments != null && vehicleCodeAssignments.Count > 0)
                {
                    ArrayList errorCodeAssignments = null;
                    foreach (VehicleTypeCodeAssignment vc in vehicleCodeAssignments)
                    {
                        if (String.Equals(errorCode, vc.ErrorCode, StringComparison.OrdinalIgnoreCase))
                        {
                            if (errorCodeAssignments == null)
                            {
                                errorCodeAssignments = new ArrayList();
                            }
                            errorCodeAssignments.Add(vc);
                        }
                    }
                }

                if (!foundErrorCodes)
                {
                    DTCMasterCodeList dtcMasterCode = null;
                    foreach (DTCMasterCodeList c in dtcMasterCodes)
                    {
                        if (String.Equals(errorCode, c.ErrorCode, StringComparison.OrdinalIgnoreCase))
                        {
                            dtcMasterCode = c;
                            break;
                        }
                    }

                    if (dtcMasterCode != null)
                    {
                        AddDiagnosticReportResultErrorCodeDefinition(dtcMasterCode);
                    }
                }
            }

            drrErrorCode.DiagnosticReportErrorCodeType = (int)diagnosticReportErrorCodeType;
            drrErrorCode.ErrorCodeType = errorCodeType;
            drrErrorCode.DiagnosticReportResultId = diagnosticReportResult.DiagnosticReportResultId;
            drrErrorCode.ErrorCode = errorCode;
            return drrErrorCode;

        }

        public void AddDiagnosticReportResultErrorCodeDefinition(DTCMasterCodeList dtcMasterCode)
        {
            DiagnosticReportResultErrorCodeDefinition def = new DiagnosticReportResultErrorCodeDefinition();
            def.DiagnosticReportResultErrorCodeId = Guid.Parse( diagnosticReportResultErrorCode[0].DiagnosticReportResultErrorCodeId);

            def.DTCMasterCodeId = Guid.Parse( dtcMasterCode.DTCMasterCodeId);
            def.Title = dtcMasterCode.Title;
            def.Title_es = dtcMasterCode.Title_Spanish;
            def.Title_fr = dtcMasterCode.Title_French;

            def.Make = GetDelimittedString(dtcMasterCode.MakeString, "|");
            this.SetLaymanTermProperties(dtcMasterCode.ErrorCode, def);
            this.unableToFindCodeData = false;
        }

        public void AddDiagnosticReportResultErrorCodeDefinition(DTCCode dtcCode)
        {
            DiagnosticReportResultErrorCodeDefinition def = new DiagnosticReportResultErrorCodeDefinition();

            // def.DiagnosticReportResultErrorCodeId = diagnosticReportResultErrorCode[0].DiagnosticReportResultErrorCodeId;
            def.DTCCodeId = Guid.Parse( dtcCode.DTCCodeId);
            def.Title = dtcCode.Title;
            def.Title_es = dtcCode.Title_es;
            def.Title_fr = dtcCode.Title_fr;
            def.Title_zh = dtcCode.Title_zh;
            def.Conditions = dtcCode.Conditions;
            def.Conditions_es = dtcCode.Conditions_es;
            def.Conditions_fr = dtcCode.Conditions_fr;
            def.Conditions_zh = dtcCode.Conditions_zh;
            def.PossibleCauses = dtcCode.PossibleCauses;
            def.PossibleCauses_es = dtcCode.PossibleCauses_es;
            def.PossibleCauses_fr = dtcCode.PossibleCauses_fr;
            def.PossibleCauses_zh = dtcCode.PossibleCauses_zh;
            def.Trips = Convert.ToInt32( dtcCode.Trips);
            def.MessageIndicatorLampFile = dtcCode.MessageIndicatorLampFile;
            def.TransmissionControlIndicatorLampFile = dtcCode.TransmissionControlIndicatorLampFile;
            def.PassiveAntiTheftIndicatorLampFile = dtcCode.PassiveAntiTheftIndicatorLampFile;
            def.ServiceThrottleSoonIndicatorLampFile = dtcCode.ServiceThrottleSoonIndicatorLampFile;
            def.MonitorType = dtcCode.MonitorType;
            def.MonitorFile = dtcCode.MonitorFile;

            def.Model = GetDelimittedString(dtcCode.ModelsString, "|");
            def.Make = GetDelimittedString(dtcCode.MakesString, "|");
            def.Year = 0;
            List<int> years = new List<int>();
            if (!isObjectCreated && dtcCode.YearsString != "")
            {
                foreach (string s in dtcCode.YearsString.Split("|".ToCharArray()))
                {
                    if (s != null && s != "")
                    {
                        years.Add(Int32.Parse(s));
                    }
                }
            }
            if (years.Count > 0)
            {
                def.Year = (int)dtcCode.YearsString[0];
            }
            def.EngineType = GetDelimittedString(dtcCode.EngineTypesString, "|");
            def.EngineVINCode = GetDelimittedString(dtcCode.EngineVINCodesString, "|");
            def.TransmissionControlType = GetDelimittedString(dtcCode.TransmissionsString, "|");

            this.SetLaymanTermProperties(dtcCode.ErrorCode, def);
            this.unableToFindCodeData = false;
        }


        private void SetLaymanTermProperties(string errorCode, DiagnosticReportResultErrorCodeDefinition def)
        {
            DTCCodeLaymanTerm dtcCodeLaymanTerm = null;
            string make = "";
            if (_diagnosticReport != null && _Vehicle != null)
            {
                make = (_objPolkVehicleYmme != null) ? _objPolkVehicleYmme.Make : _Vehicle.Make;
            }
            dtcCodeLaymanTerm = _getMostLikelyFixRepository.GetDTCCodeLaymanTermLoadByErrorCodeAndMake(errorCode, make);
            if (dtcCodeLaymanTerm != null)
            {
                def.LaymansTermTitle = dtcCodeLaymanTerm.Title;
                def.LaymansTermTitle_es = dtcCodeLaymanTerm.Title_es;
                def.LaymansTermTitle_fr = dtcCodeLaymanTerm.Title_fr;
                def.LaymansTermTitle_zh = dtcCodeLaymanTerm.Title_zh;
                def.LaymansTermDescription = dtcCodeLaymanTerm.Description;
                def.LaymansTermDescription_es = dtcCodeLaymanTerm.Description_es;
                def.LaymansTermDescription_fr = dtcCodeLaymanTerm.Description_fr;
                def.LaymansTermDescription_zh = dtcCodeLaymanTerm.Description_zh;
                def.LaymansTermConditions = dtcCodeLaymanTerm.Description;
                def.LaymansTermConditions_es = dtcCodeLaymanTerm.Description_es;
                def.LaymansTermConditions_fr = dtcCodeLaymanTerm.Description_fr;
                def.LaymansTermConditions_zh = dtcCodeLaymanTerm.Description_zh;
                def.LaymansTermSeverityLevel = Convert.ToInt32(dtcCodeLaymanTerm.SeverityLevel);
                def.LaymansTermEffectOnVehicle = dtcCodeLaymanTerm.EffectOnVehicle;
                def.LaymansTermEffectOnVehicle_es = dtcCodeLaymanTerm.EffectOnVehicle_es;
                def.LaymansTermEffectOnVehicle_fr = dtcCodeLaymanTerm.EffectOnVehicle_fr;
                def.LaymansTermEffectOnVehicle_zh = dtcCodeLaymanTerm.EffectOnVehicle_zh;
                def.LaymansTermResponsibleComponentOrSystem = dtcCodeLaymanTerm.ResponsibleComponentOrSystem;
                def.LaymansTermResponsibleComponentOrSystem_es = dtcCodeLaymanTerm.ResponsibleComponentOrSystem_es;
                def.LaymansTermResponsibleComponentOrSystem_fr = dtcCodeLaymanTerm.ResponsibleComponentOrSystem_fr;
                def.LaymansTermResponsibleComponentOrSystem_zh = dtcCodeLaymanTerm.ResponsibleComponentOrSystem_zh;
                def.LaymansTermWhyItsImportant = dtcCodeLaymanTerm.WhyItsImportant;
                def.LaymansTermWhyItsImportant_es = dtcCodeLaymanTerm.WhyItsImportant_es;
                def.LaymansTermWhyItsImportant_fr = dtcCodeLaymanTerm.WhyItsImportant_fr;
                def.LaymansTermWhyItsImportant_zh = dtcCodeLaymanTerm.WhyItsImportant_zh;
            }
        }

        private static string GetDelimittedString(IEnumerable list, string delimiter)
        {
            string s = "";

            foreach (object o in list)
            {
                string os = o.ToString();

                if (os != null && os != "")
                {
                    if (s.Length > 0)
                    {
                        s += delimiter;
                    }
                    s += os.Trim();
                }
            }
            return s;
        }


        private List<VehicleTypeCodeAssignment> GetDelmarVehicleCodeAssignments(List<VehicleType> vehicleTypes, List<string> errorCodes)
        {
            return _getMostLikelyFixRepository.GetVehicleTypeCodeAssignment(vehicleTypes, errorCodes);
        }

        public List<VehicleType> GetDelmarVehicleTypes()
        {
            return this.SearchForDelmarVehicleTypes(Convert.ToInt32( _Vehicle.Year), _Vehicle.Make, _Vehicle.Model, _Vehicle.TransmissionControlType, _Vehicle.EngineVINCode, _Vehicle.EngineType, BodyCode);
        }

        private List<VehicleType> SearchForDelmarVehicleTypes(int modelYear, string make, string model, string transmissionType, string engineVINCode, string engineType, string bodyCode)
        {
            List<VehicleType> vehicleTypes = null;

            if (bodyCode != null)
            {
                vehicleTypes = this.GetDelmarVehicleTypes(modelYear, make, model, engineVINCode, transmissionType, engineType, bodyCode);
            }

            if (vehicleTypes == null || vehicleTypes.Count == 0)
            {
                vehicleTypes = this.GetDelmarVehicleTypes(modelYear, make, model, engineVINCode, transmissionType, engineType, null);

                if (vehicleTypes == null || vehicleTypes.Count == 0)
                {
                    vehicleTypes = this.GetDelmarVehicleTypes(modelYear, make, model, engineVINCode, transmissionType, null, null);

                    if (vehicleTypes == null || vehicleTypes.Count == 0)
                    {
                        vehicleTypes = this.GetDelmarVehicleTypes(modelYear, make, model, engineVINCode, null, null, null);

                        if (vehicleTypes == null || vehicleTypes.Count == 0)
                        {
                            vehicleTypes = this.GetDelmarVehicleTypes(modelYear, make, model, null, null, null, null);
                        }
                    }
                }
            }
            return vehicleTypes;
        }

        private List<VehicleType> GetDelmarVehicleTypes(int modelYear, string make, string model, string engineVINCode, string transmissionType, string engineType, string bodyCode)
        {
          return _getMostLikelyFixRepository.GetVehicleType_LoadByVinData(modelYear, make, model, engineVINCode, transmissionType, engineType, bodyCode);
        }
        public string BodyCode
        {
            get
            {
                if (_Vehicle.ManufacturerName.IndexOf("General") >= 0)
                {
                    Regex regex = new Regex(@"\d");
                    if (!regex.IsMatch(_Vehicle.Vin.Substring(2, 1)))
                    {
                        bodyCode = _Vehicle.Vin.Substring(4, 1);
                    }
                }
                return bodyCode;
            }
        }

        private List<DTCMasterCodeList> GetDtcMasterCodes(List<string> errorCodes)
        {
             return  _getMostLikelyFixRepository.GetDTCMasterCodeList_LoadByDiagnosticReportAndErrorCodes(_Vehicle.Make, errorCodes);
        }

        public List<string> AbsAllCodes
        {
            get
            {
                return this.GetAllErrorCodes(DiagnosticReportErrorCodeSystemType.ABS);
            }
        }

        public List<string> Obd1AllCodes
        {
            get
            {
                return this.GetAllErrorCodes(DiagnosticReportErrorCodeSystemType.PowertrainOBD1);
            }
        }

        public List<string> SrsAllCodes
        {
            get
            {
                return this.GetAllErrorCodes(DiagnosticReportErrorCodeSystemType.SRS);
            }
        }

        public List<string> GetAllErrorCodes(DiagnosticReportErrorCodeSystemType systemType)
        {
            List<string> codes = new List<string>();
            string milDtc = "";
            List<string> storedCodes = this.GetStoredErrorCodes(systemType);
            List<string> pendingCodes = this.GetPendingErrorCodes(systemType);

            if (systemType == DiagnosticReportErrorCodeSystemType.PowertrainObd2)
            {
                milDtc = _diagnosticReport.PwrMilCode;
            }

            if (milDtc != "")
            {
                codes.Add(milDtc);
            }

            foreach (string code in storedCodes)
            {
                if (!string.IsNullOrEmpty(code) && !codes.Contains(code))
                {
                    codes.Add(code);
                }
            }
            foreach (string code in pendingCodes)
            {
                if (!string.IsNullOrEmpty(code) && !codes.Contains(code))
                {
                    codes.Add(code);
                }
            }

            return codes;
        }
        public List<string> GetStoredErrorCodes(DiagnosticReportErrorCodeSystemType systemType)
        {
            List<string> storedCodes = new List<string>();
            DtcCodeViewCollection Obd1StoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1StoredCodesString);
            DtcCodeViewCollection AbsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsStoredCodesString);
            DtcCodeViewCollection SrsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsStoredCodesString);

            switch (systemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    storedCodes = AbsStoredCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    storedCodes = Obd1StoredCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    storedCodes = this.PwrStoredCodes;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    storedCodes = SrsStoredCodes.AllDtcs;
                    break;
            }

            return storedCodes;
        }
        public List<string> GetPendingErrorCodes(DiagnosticReportErrorCodeSystemType systemType)
        {
            List<string> pendingCodes = new List<string>();
            DtcCodeViewCollection AbsPendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsPendingCodesString);
            DtcCodeViewCollection Obd1PendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1PendingCodesString);
            DtcCodeViewCollection SrsPendingCodes = GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsPendingCodesString);
            switch (systemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    pendingCodes = AbsPendingCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    pendingCodes = Obd1PendingCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    pendingCodes = this.PwrPendingCodes;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    pendingCodes = SrsPendingCodes.AllDtcs;
                    break;
            }
            return pendingCodes;
        }

        private List<Fix> GetFixesSorted(string primaryErrorCode, List<string> secondaryDtcs, bool logDiscrepancies, bool onlyProcessFixes, DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType)
        {
            List<Fix> fixesPolk = new List<Fix>();
            fixesPolk = _getMostLikelyFixRepository.GetFix_LoadByDiagnosticReportBySymptom(Convert.ToInt32(_objPolkVehicleYmme.Year), _objPolkVehicleYmme.Make, _objPolkVehicleYmme.Model, _objPolkVehicleYmme.Trim, _objPolkVehicleYmme.Transmission, primaryErrorCode, _objPolkVehicleYmme.EngineVinCode, _objPolkVehicleYmme.EngineType, 0, "US");
            List<Fix> fixesVinPower = new List<Fix>();
            fixesVinPower = _getMostLikelyFixRepository.GetFix_LoadByDiagnosticReportUsingVinPower(Convert.ToInt32(_Vehicle.VPYear), _Vehicle.VPMake, _Vehicle.VPModel, _Vehicle.VPTrimLevel, _Vehicle.TransmissionControlType, primaryErrorCode, _Vehicle.VPEngineVINCode, _Vehicle.VPEngineType, 0);

            List<Fix> fixes = new List<Fix>();
            foreach (Fix f in fixesVinPower)
            {
                Fix polkFix = (Fix)fixesPolk.Where(x => x.FixId == f.FixId);

                if (polkFix == null)
                {
                    if (logDiscrepancies && _objPolkVehicleYmme != null)
                    {
                        AddDiscrepancy(f, _objPolkVehicleYmme, true, false);
                    }
                }
                else
                {
                    IsFromPolkMatch = true;
                }
                fixes.Add(f);
            }

            foreach (Fix f in fixesPolk)
            {
                Fix vinPowerFix = (Fix)fixesVinPower.Where(x => x.FixId == f.FixId);

                if (vinPowerFix == null)
                {
                    if (logDiscrepancies && _objPolkVehicleYmme != null)
                    {
                        AddDiscrepancy(f, _objPolkVehicleYmme, false, true);
                    }
                }
                else
                {
                    IsFromVinPowerMatch = true;
                }
                if (fixes.Where(x => x.FixId == f.FixId) == null)
                {
                    fixes.Add(f);
                }
            }
            return fixes;
        }

        public void SetFixStatuses(DiagnosticReportErrorCodeSystemType systemType)
        {
            List<string> storedCodes = new List<string>();
            bool isPwrLookForFix = (_diagnosticReport.ToolMilStatus == (int)ToolMilStatus.On && _diagnosticReport.ToolLEDStatus == (int)ToolLEDStatus.Red && !string.IsNullOrEmpty(_diagnosticReport.PwrMilCode));
            DiagnosticReportFixStatus fixStatus = DiagnosticReportFixStatus.FixNotFound;
            bool isFixFeedbackRequired = false;

            DtcCodeViewCollection Obd1StoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.Obd1StoredCodesString);
            DtcCodeViewCollection AbsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.AbsStoredCodesString);
            DtcCodeViewCollection SrsStoredCodes = this.GetDtcCodeViewCollectionFromDelimitedString(_diagnosticReport.SrsStoredCodesString);

            switch (systemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    storedCodes = AbsStoredCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    storedCodes = Obd1StoredCodes.AllDtcs;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    storedCodes = this.PwrStoredCodes;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    storedCodes = SrsStoredCodes.AllDtcs;
                    break;
            }

            if (systemType == DiagnosticReportErrorCodeSystemType.PowertrainObd2)
            {
                if (!isPwrLookForFix)
                {
                    fixStatus = DiagnosticReportFixStatus.FixNotNeeded;
                }
                else
                {
                    if (GetFixesBySystemType(systemType).Count == 0)
                    {
                        fixStatus = DiagnosticReportFixStatus.FixNotFound;
                        isFixFeedbackRequired = true;
                    }
                    else
                    {
                        fixStatus = DiagnosticReportFixStatus.FixFound;
                    }
                }
            }
            else
            {
                if (storedCodes.Count == 0)
                {
                    fixStatus = DiagnosticReportFixStatus.FixNotNeeded;
                }
                else
                {
                    bool allCodesHaveAFix = true;
                    isFixFeedbackRequired = true;

                    foreach (string errorCode in storedCodes)
                    {

                        ArrayList fixes = null;
                        foreach (DiagnosticReportResultFix f in diagnosticReportResultFixes)
                        {
                            if (f.DiagnosticReportErrorCodeSystemType == (int)systemType)
                            {
                                if (String.Equals(f.PrimaryErrorCode, errorCode, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (fixes == null)
                                    {
                                        fixes = new ArrayList();
                                    }
                                    fixes.Add(f);
                                }
                            }
                        }

                        if (fixes == null || fixes.Count == 0)
                        {
                            allCodesHaveAFix = false;
                            break;
                        }
                        else
                        {
                            isFixFeedbackRequired = false;
                        }
                    }

                    if (allCodesHaveAFix)
                    {
                        fixStatus = DiagnosticReportFixStatus.FixFound;
                    }
                    else
                    {
                        fixStatus = DiagnosticReportFixStatus.FixNotFound;
                    }
                }
            }

            switch (systemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    this.AbsDiagnosticReportFixStatus = fixStatus;
                    _diagnosticReport.IsAbsFixFeedbackRequired = isFixFeedbackRequired;
                    _diagnosticReport.AbsDiagnosticReportFixStatusWhenCreated = (int)this.AbsDiagnosticReportFixStatus;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    this.Obd1DiagnosticReportFixStatus = fixStatus;
                    _diagnosticReport.IsPwrObd1FixFeedbackRequired = isFixFeedbackRequired;
                    _diagnosticReport.Obd1DiagnosticReportFixStatusWhenCreated = (int)this.Obd1DiagnosticReportFixStatus;

                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    this.PwrDiagnosticReportFixStatus = fixStatus;
                    _diagnosticReport.IsPwrObd2FixFeedbackRequired = isFixFeedbackRequired;
                    _diagnosticReport.PwrDiagnosticReportFixStatusWhenCreated = (int)this.PwrDiagnosticReportFixStatus;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    this.SrsDiagnosticReportFixStatus = fixStatus;
                    _diagnosticReport.IsSrsFixFeedbackRequired = isFixFeedbackRequired;
                    _diagnosticReport.SrsDiagnosticReportFixStatusWhenCreated = (int)this.SrsDiagnosticReportFixStatus;

                    break;
            }
        }

        private List<SymptomDiagnosticReportItem> SymptomDiagnosticReportItem
        {
            get
            {
                List<SymptomDiagnosticReportItem> symptomDiagnosticReportItem = new List<SymptomDiagnosticReportItem>();
                if (SymptomDiagnosticReportItem == null)
                {
                    var symptomReports = _getMostLikelyFixRepository.GetSymptomDiagnosticReportItemByDiagnosticReportId(_diagnosticReport.DiagnosticReportId);

                    if (symptomReports != null && symptomReports.Count > 0)
                    {
                        foreach (SymptomDiagnosticReportItem symptomDiagnostic in symptomReports)
                        {
                            symptomDiagnosticReportItem.Add(symptomDiagnostic);
                        }
                    }
                }
                return symptomDiagnosticReportItem;
            }
        }

        public void CreateDiagnosticReportResultFixForSymptoms()
        {
            bool fixFound = false;

            if (this.SymptomGuids != null && this.SymptomGuids.Any())
            {
                foreach (var symptomId in this.SymptomGuids)
                {
                    fixFound = this.CreateDiagnosticReportResultForSymptoms(symptomId, DiagnosticReportErrorCodeSystemType.PowertrainObd2, false, false);

                    if (!this.pwrFixFoundAfterLastFixLookup)
                    {
                        this.pwrFixFoundAfterLastFixLookup = fixFound;
                    }
                }
            }

            //this.SetFixStatusesForSymptoms(DiagnosticReportErrorCodeSystemType.PowertrainObd2);

            //if (fixFound && this.FixProvidedDateTimeUTC == null)
            //{
            //    this.FixProvidedDateTimeUTC = DateTime.UtcNow;
            //}
        }

        private bool CreateDiagnosticReportResultForSymptoms(string symptomId, DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType, bool logDiscrepancies, bool onlyProcessFixes)
        {
            bool newFixFound = false;
            DiagnosticReportFixStatus oldFixStatus = DiagnosticReportFixStatus.FixNotFound;

            if (diagnosticReportErrorCodeSystemType != DiagnosticReportErrorCodeSystemType.Enhanced)
            {
                if (symptomId == null || symptomId == string.Empty)
                {
                    return false;
                }

                List<Fix> fixes = new List<Fix>();

                switch (diagnosticReportErrorCodeSystemType)
                {
                    case DiagnosticReportErrorCodeSystemType.PowertrainObd2:

                        fixes = GetFixesSortedForSymptom(symptomId, logDiscrepancies, onlyProcessFixes, diagnosticReportErrorCodeSystemType);
                        oldFixStatus = PwrDiagnosticReportFixStatus;
                        break;
                }

                if (!onlyProcessFixes)
                {
                    diagnosticReportResultFixes = _getMostLikelyFixRepository.GetDiagnosticReportResultFix("diagnosticReport.DiagnosticReportResultId");
                    for (int i = 0; i < fixes.Count; i++)
                    {
                        Fix fix = fixes[i];
                        DiagnosticReportResultFix drrFix = SetDiagnosticReportResultFix(fix);
                        drrFix.PrimaryErrorCode = string.Empty; //No DTC here, just for Symptom
                        drrFix.DiagnosticReportErrorCodeSystemType = 0;
                        drrFix.SortOrder = i;
                        diagnosticReportResultFixes.Add(drrFix);
                    }
                }
            }

            if (!onlyProcessFixes)
            {
                this.SetFixStatusesForSymptoms(diagnosticReportErrorCodeSystemType);
                DiagnosticReportFixStatus newFixStatus = this.GetFixStatus(diagnosticReportErrorCodeSystemType);

                if (newFixStatus == DiagnosticReportFixStatus.FixFound && newFixStatus != oldFixStatus)
                {
                    newFixFound = true;
                }
            }
            return newFixFound;
        }

        public void SetFixStatusesForSymptoms(DiagnosticReportErrorCodeSystemType systemType)
        {
            bool isPwrLookForFix = this.SymptomGuids != null && this.SymptomGuids.Any();

            DiagnosticReportFixStatus fixStatus = DiagnosticReportFixStatus.FixNotFound;
            bool isFixFeedbackRequired = false;

            if (systemType == DiagnosticReportErrorCodeSystemType.PowertrainObd2)
            {
                if (!isPwrLookForFix)
                {
                    fixStatus = DiagnosticReportFixStatus.FixNotNeeded;
                }
                else
                {
                    if (GetFixesBySystemType(systemType).Count == 0)
                    {
                        fixStatus = DiagnosticReportFixStatus.FixNotFound;
                        isFixFeedbackRequired = true;
                    }
                    else
                    {
                        fixStatus = DiagnosticReportFixStatus.FixFound;
                    }
                }
            }
            switch (systemType)
            {
                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    this.PwrDiagnosticReportFixStatus = fixStatus;
                    _diagnosticReport.IsPwrObd2FixFeedbackRequired = isFixFeedbackRequired;
                    if (this.isObjectCreated)
                    {
                        _diagnosticReport.PwrDiagnosticReportFixStatusWhenCreated = (int)this.PwrDiagnosticReportFixStatus;
                    }
                    break;
            }
        }

        private List<DiagnosticReportResultFix> GetFixesBySystemType(DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType)
        {
            List<DiagnosticReportResultFix> drFixes = new List<DiagnosticReportResultFix>();

            switch (diagnosticReportErrorCodeSystemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    drFixes = this.AbsDiagnosticReportResultFixes;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    drFixes = this.PwrObd1DiagnosticReportResultFixes;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    drFixes = this.PwrObd2DiagnosticReportResultFixes;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    drFixes = this.SrsDiagnosticReportResultFixes;
                    break;
            }
            return drFixes;
        }
        public DiagnosticReportFixStatus GetFixStatus(DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType)
        {
            DiagnosticReportFixStatus fixStatus = DiagnosticReportFixStatus.FixFound;

            switch (diagnosticReportErrorCodeSystemType)
            {
                case DiagnosticReportErrorCodeSystemType.ABS:
                    fixStatus = this.AbsDiagnosticReportFixStatus;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainOBD1:
                    fixStatus = this.Obd1DiagnosticReportFixStatus;
                    break;

                case DiagnosticReportErrorCodeSystemType.PowertrainObd2:
                    fixStatus = this.PwrDiagnosticReportFixStatus;
                    break;

                case DiagnosticReportErrorCodeSystemType.SRS:
                    fixStatus = this.SrsDiagnosticReportFixStatus;
                    break;
            }
            return fixStatus;
        }
        public DiagnosticReportResultFix SetDiagnosticReportResultFix(Fix fix)
        {
            var FixPart = _getMostLikelyFixRepository.GetFixPartByFixId(fix.FixId);
            CalculateFixCosts(FixPart, fix);
            FixName fixName = _getMostLikelyFixRepository.GetByFixNameId(fix.FixNameId);
            DiagnosticReportResultFix drrFix = new DiagnosticReportResultFix();
            drrFix.AdditionalCost = fix.AdditionalCost;
            drrFix.AdditionalCostInLocalCurrency = GetLocalCurrencyValueFromUSDollars((decimal)fix.AdditionalCost);
            drrFix.Description = fix.Description;
            drrFix.DiagnosticReportErrorCodeSystemType = 0;
            drrFix.DiagnosticReportIsExactMatch = true;
            drrFix.DiagnosticReportSecondaryCodeAssignmentMatches = 0;
            drrFix.FrequencyCount = fix.FrequencyCount;
            drrFix.Labor = fix.Labor;
            drrFix.LaborCost = this.laborCost;
            drrFix.LaborCostInLocalCurrency = this.laborCostInLocalCurrency;
            drrFix.LaborRate = this.laborRate;
            drrFix.LaborRateInLocalCurrency = this.laborRateInLocalCurrency;
            drrFix.Name = fixName.Description;
            drrFix.Name_es = fixName.Description_es;
            drrFix.Name_fr = fixName.Description_fr;
            drrFix.Name_zh = fixName.Description_zh;
            drrFix.PartsCost = this.partsCost;
            drrFix.PartsCostInLocalCurrency = this.partsCostInLocalCurrency;
            drrFix.TotalCost = this.totalCost;
            drrFix.TotalCostInLocalCurrency = this.totalCostInLocalCurrency;
            return drrFix;
        }

        private List<DiagnosticReportResultFix> AbsDiagnosticReportResultFixes
        {
            get
            {
                List<DiagnosticReportResultFix> fixes = new List<DiagnosticReportResultFix>();
                var tempFixes = this.diagnosticReportResultFixes.Where(x => x.DiagnosticReportErrorCodeSystemType == Convert.ToInt32(DiagnosticReportErrorCodeSystemType.ABS)).ToList();
                if (tempFixes != null && tempFixes.Count > 0)
                {
                    foreach (DiagnosticReportResultFix drrFix in tempFixes)
                    {
                        fixes.Add(drrFix);
                    }
                }
                return fixes;
            }
        }

        private List<DiagnosticReportResultFix> PwrObd1DiagnosticReportResultFixes
        {
            get
            {
                List<DiagnosticReportResultFix> fixes = new List<DiagnosticReportResultFix>();
                var tempFixes = this.diagnosticReportResultFixes.Where(x => x.DiagnosticReportErrorCodeSystemType == Convert.ToInt32(DiagnosticReportErrorCodeSystemType.PowertrainOBD1)).ToList();
                if (tempFixes != null && tempFixes.Count > 0)
                {
                    foreach (DiagnosticReportResultFix drrFix in tempFixes)
                    {
                        fixes.Add(drrFix);
                    }
                }
                return fixes;
            }
        }

        private List<DiagnosticReportResultFix> PwrObd2DiagnosticReportResultFixes
        {
            get
            {
                List<DiagnosticReportResultFix> fixes = new List<DiagnosticReportResultFix>();
                var tempFixes = this.diagnosticReportResultFixes.Where(x => x.DiagnosticReportErrorCodeSystemType == Convert.ToInt32(DiagnosticReportErrorCodeSystemType.PowertrainObd2)).ToList();
                if (tempFixes != null && tempFixes.Count > 0)
                {
                    foreach (DiagnosticReportResultFix drrFix in tempFixes)
                    {
                        fixes.Add(drrFix);
                    }
                }
                return fixes;
            }
        }

        private List<DiagnosticReportResultFix> SrsDiagnosticReportResultFixes
        {
            get
            {
                List<DiagnosticReportResultFix> fixes = new List<DiagnosticReportResultFix>();
                var tempFixes = this.diagnosticReportResultFixes.Where(x => x.DiagnosticReportErrorCodeSystemType == Convert.ToInt32(DiagnosticReportErrorCodeSystemType.SRS)).ToList();
                if (tempFixes != null && tempFixes.Count > 0)
                {
                    foreach (DiagnosticReportResultFix drrFix in tempFixes)
                    {
                        fixes.Add(drrFix);
                    }
                }
                return fixes;
            }
        }


        public Decimal GetLocalCurrencyValueFromUSDollars(Decimal usDollarsValue)
        {
            return this.ConvertCurrency(Currency.USD, usDollarsValue, _currentCurrency);
        }

        public Decimal ConvertCurrency(Currency sourceCurrencyType, Decimal sourceCurrencyValue, Currency targetCurrencyType)
        {
            if (sourceCurrencyType == targetCurrencyType)
                return sourceCurrencyValue;
            if (targetCurrencyType != Currency.USD && sourceCurrencyType != Currency.USD)
                return this.ConvertCurrency(Currency.USD, this.ConvertCurrency(sourceCurrencyType, sourceCurrencyValue, Currency.USD), targetCurrencyType);
            decimal byProperty1 = FindExchangeRatePerUSDByCurrenctISOCode(Convert.ToString(sourceCurrencyType));
            decimal byProperty2 = FindExchangeRatePerUSDByCurrenctISOCode(Convert.ToString(targetCurrencyType));
            if (targetCurrencyType == Currency.USD)
                return Math.Round(sourceCurrencyValue / byProperty1, 2);
            return Math.Round(sourceCurrencyValue * byProperty2, 2);
        }

        public decimal FindExchangeRatePerUSDByCurrenctISOCode(string CurrenctISOCode)
        {
            return _getMostLikelyFixRepository.GetCurrencyExchangeRate(CurrenctISOCode);
        }

        public void CalculateFixCosts(List<FixPart> fixPart, Fix fix)
        {
            decimal rate = 0;
            var stateLaborRate = _getMostLikelyFixRepository.GetStateLaborByStateCode(StateCode);
            if (stateLaborRate != null)
            {
                rate =  (decimal)stateLaborRate.DollarsPerHour;
            }
            this.laborRate = rate;
            this.laborCost = (decimal)fix.Labor * this.laborRate;
            this.partsCost = 0;

            foreach (FixPart p in fixPart)
            {
                var part = _getMostLikelyFixRepository.GetPartByPartId(p.PartId);
                partsCost += Convert.ToDecimal(p.Quantity) * Convert.ToDecimal(part.Price);
            }

            this.totalCost = this.laborCost + (decimal)fix.AdditionalCost + this.partsCost;

            if (_currentCurrency != Currency.USD)
            {
                this.laborRateInLocalCurrency = GetLocalCurrencyValueFromUSDollars(this.laborRate);
                this.laborCostInLocalCurrency = GetLocalCurrencyValueFromUSDollars(this.laborCost);
                this.additionalCostInLocalCurrency = GetLocalCurrencyValueFromUSDollars((decimal)fix.AdditionalCost);
                this.partsCostInLocalCurrency = GetLocalCurrencyValueFromUSDollars(this.partsCost);
                this.totalCostInLocalCurrency = GetLocalCurrencyValueFromUSDollars(this.totalCost);
            }
        }
        private List<Fix> GetFixesSortedForSymptom(string symptomId, bool logDiscrepancies, bool onlyProcessFixes, DiagnosticReportErrorCodeSystemType diagnosticReportErrorCodeSystemType)
        {
            List<Fix> fixesPolk = new List<Fix>();
            List<Fix> fixesVinPower = new List<Fix>();

            if (_objPolkVehicleYmme != null)
            {
                fixesPolk = _getMostLikelyFixRepository.GetFix_LoadByDiagnosticReportBySymptom(Convert.ToInt32( _objPolkVehicleYmme.Year), _objPolkVehicleYmme.Make, _objPolkVehicleYmme.Model, _objPolkVehicleYmme.Trim, _objPolkVehicleYmme.Transmission, symptomId, _objPolkVehicleYmme.EngineVinCode, _objPolkVehicleYmme.EngineType,0, "US");
            }


            fixesVinPower = _getMostLikelyFixRepository.GetLoadByDiagnosticReportUsingVinPowerBySymptom(Convert.ToInt32(_Vehicle.VPYear), _Vehicle.VPMake, _Vehicle.VPModel, _Vehicle.VPTrimLevel, _Vehicle.TransmissionControlType, symptomId, _Vehicle.VPEngineVINCode, _Vehicle.VPEngineType);
            List<Fix> fixes = new List<Fix>();
            foreach (Fix f in fixesVinPower)
            {
                Fix polkFix = (Fix)fixesPolk.Where(x => x.FixId == f.FixId);
                if (polkFix == null)
                {
                    if (logDiscrepancies && _objPolkVehicleYmme != null)
                    {
                        AddDiscrepancy(f, this._objPolkVehicleYmme, true, false);
                    }
                }
                else
                {
                    IsFromPolkMatch = true;
                }
                fixes.Add(f);
            }

            foreach (Fix f in fixesPolk)
            {
                Fix vinPowerFix = (Fix)fixesVinPower.Where(x => x.FixId == f.FixId);
                if (vinPowerFix == null)
                {
                    if (logDiscrepancies && this._objPolkVehicleYmme != null)
                    {
                        AddDiscrepancy(f, this._objPolkVehicleYmme, false, true);
                    }
                }
                else
                {
                    IsFromVinPowerMatch = true;
                }

                if (fixes.Where(x => x.FixId == f.FixId) == null)
                {
                    fixes.Add(f);
                }
            }

            return fixes;
        }

        public void AddDiscrepancy(Fix fix, PolkVehicleYmme polkVehicleYMME, bool isPolkDiscrepancy, bool isVinPowerDiscrepancy)
        {
            FixPolkVehicleDiscrepancy fpd = GetDiscrepancy(fix, polkVehicleYMME);

            if (fpd == null)
            {
                fpd = new FixPolkVehicleDiscrepancy();
                fpd.FixId = fix.FixId;
                fpd.PolkVehicleYMMEId = polkVehicleYMME.PolkVehicleYMMEId;
            }
            else
            {
                fpd.IsDeleted = false;
                AdminUserDeletedBy = null;
            }

            if (isPolkDiscrepancy)
            {
                fpd.OccurrencesOfPolkMissing++;
            }

            if (isVinPowerDiscrepancy)
            {
                fpd.OccurrencesOfVinPowerMissing++;
            }

            fpd.UpdatedDateTimeUTC = DateTime.UtcNow;
            _getMostLikelyFixRepository.SaveFixPolkVehicleDiscrepancy(fpd);
        }

        public FixPolkVehicleDiscrepancy GetDiscrepancy(Fix fix, PolkVehicleYmme polkVehicleYMME)
        {
             return _getMostLikelyFixRepository.GetFixPolkVehicleDiscrepancy_LoadByFixAndVehicle(fix.FixId, polkVehicleYMME.PolkVehicleYMMEId);
        }

        private List<DTCCode> GetDtcCodes(List<string> errorCodes)
        {
            return  _getMostLikelyFixRepository.GetDTCCode_LoadByDiagnosticReportAndErrorCodes(Convert.ToInt32(_Vehicle.Year), _Vehicle.Make, _Vehicle.Model, _Vehicle.TransmissionControlType, _Vehicle.EngineType, _Vehicle.EngineVINCode, _Vehicle.TrimLevel, errorCodes);
        }

        private bool ToBoolean(string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    throw new InvalidCastException("You can't cast that value to a bool!");
            }
        }

        public virtual  DiagReportInfo GetDiagnosticReportExisting(
                      int diagnosticReportId,
                      bool includeRecallsForVehicle,
                      bool includeTSBsForVehicleAndMatchingErrorCodes,
                      bool includeTSBCountForVehicle,
                      bool includeNextScheduledMaintenance,
                      bool includeWarrantyInfo,
                      bool autoZone = false)
        {
            return new DiagReportInfo(); ;
        }

    }
}
