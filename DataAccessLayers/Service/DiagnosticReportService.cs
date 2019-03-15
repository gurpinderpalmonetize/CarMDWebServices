using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayers.WebService;
using DataAccessLayers.DataObjects;
using System.Xml;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Reflection;
using DataAccessLayers.DataBase;
using DataAccessLayers.Common;

namespace DataAccessLayers.Service
{
    public class DiagnosticReportService
    {
        string StateCode = "QC";
        Currency currentCurrency;
        public Guid? ValidateKeyAndLogTransaction(string key)
        {
            var externalSystem = new ExternalSystemWebService();
            Guid? id = ValidateKey(key);
            if (id.HasValue) { }
            return id;
        }
        private Guid? ValidateKey(string key)
        {
            var externalSystem = ExternalSystemWebService.GetActiveExternalSystemFromKey(key);

            Guid? id = null;

            if (externalSystem.KeyGuid != null)
            {
                id = externalSystem.KeyGuid.Value;

                if (id == Guid.Empty)
                {
                    id = null;
                }
            }
            else
            {
                id = Guid.Empty;
                id = null;
            }
            return id;
        }
        public VehicleInfoModel GetVehicleInfoByVin(string vin)
        {
            var VehicleInfo = new VehicleInfoModel();
            string errorMessage = "";
            try
            {
                if (vin == null)
                {
                    VehicleInfo.ValidationFailures = new ValidationFailureModel[1];
                    VehicleInfo.ValidationFailures[0] = new ValidationFailureModel();
                    VehicleInfo.ValidationFailures[0].Code = "426";
                    VehicleInfo.ValidationFailures[0].Description = "The VIN supplied is invalid, you cannot retreive a diagnostic report without a valid vin";
                    VehicleInfo.IsValid = false;
                }
                else
                {
                    var vinDecoder = DecodeVIN(vin, false);
                    VehicleInfo.VIN = vinDecoder.VinPatternMask;
                    VehicleInfo.ManufacturerName = vinDecoder.Manufacturer;
                    VehicleInfo.Make = vinDecoder.Make;
                    VehicleInfo.Year = vinDecoder.Year.ToString();
                    VehicleInfo.Model = vinDecoder.Model;
                    VehicleInfo.Transmission = vinDecoder.Transmission;
                    VehicleInfo.EngineType = vinDecoder.EngineType;
                    VehicleInfo.TrimLevel = vinDecoder.Trim;
                    VehicleInfo.EngineVINCode = vinDecoder.EngineVinCode;
                    VehicleInfo.AAIA = vinDecoder.AAIA;
                    VehicleInfo.ModelImageFileUrl = GlobalModel.PolkVehicleImageRootUrl;
                }


                return VehicleInfo;
            }
            catch (Exception ex)
            {
                errorMessage = "There was an error decoding the vin: " + ex.Message;
            }
            return VehicleInfo;
        }
        public PolkVehicleYmme DecodeVIN(string vin, bool validateVin)
        {
            PolkVehicleYmme DecodedPolkVehicle = new PolkVehicleYmme();
            List<string> stringList = new List<string>();
            vin = vin.Replace(" ", string.Empty);
            object polkVehicleYmmeObj = (object)null;

            using (innovaEntities _context = new innovaEntities())
            {
                if (!validateVin || IsVinValid(vin))
                {
                    string lower = vin.ToLower();
                    switch (vin.Length)
                    {
                        case 4:
                        case 5:
                        case 6:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower + "******")).FirstOrDefault();
                            break;
                        case 7:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower + "******")).FirstOrDefault()
                                ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 3) + "****")).FirstOrDefault();
                            break;
                        case 8:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower + "******")).FirstOrDefault()
                               ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 3) + "*****")).FirstOrDefault();
                            break;
                        case 10:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 8) + "*" + lower.Substring(9, 1) + "******")).FirstOrDefault()
                               ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 4) + "******")).FirstOrDefault();
                            break;
                        case 11:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 8) + "*" + lower.Substring(9, 2) + "******")).FirstOrDefault()
                              ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 8) + "*" + lower.Substring(9, 1) + "*******")).FirstOrDefault()
                              ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 5) + "******")).FirstOrDefault();
                            break;
                        case 12:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 6) + "******")).FirstOrDefault()
                               ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 5) + "*******")).FirstOrDefault();
                            break;
                        case 13:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 7) + "******")).FirstOrDefault()
                               ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 6) + "*******")).FirstOrDefault();
                            break;
                        case 14:
                            polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 8) + "******")).FirstOrDefault()
                               ?? _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == (lower.Substring(0, 6) + "********")).FirstOrDefault();
                            break;
                        case 17:
                            string str1 = lower.Substring(0, 8);
                            for (int length = 5; length > 0; --length)
                            {
                                string str2 = "".PadRight(8 - length, '*');
                                string str3 = str1 + "*" + lower.Substring(9, length) + str2;
                                stringList.Add(str3);
                                polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == str3).FirstOrDefault();
                                if (polkVehicleYmmeObj != null)
                                    break;
                            }
                            if (lower.Contains("*") && polkVehicleYmmeObj == null)
                            {
                                for (int length = 5; length > 0; --length)
                                {
                                    Regex rx = new Regex("^" + str1 + "\\*" + lower.Substring(9, length).Replace("*", "\\*"));
                                    string str2 = _context.PolkVehicleYmmes.Where(x => rx.IsMatch(x.VinPatternMask.ToLower())).FirstOrDefault()?.VinPatternMask.ToLower();
                                    if (!string.IsNullOrEmpty(str2))
                                    {
                                        stringList.Add(str2);
                                        polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == str2).FirstOrDefault();
                                        break;
                                    }
                                }
                                break;
                            }
                            break;
                    }
                }
                if (polkVehicleYmmeObj == null && !IsVinAValidMaskPattern(vin))
                {
                    foreach (string vinPatternMask in GenerateVinPatternMasks(vin))
                    {
                        polkVehicleYmmeObj = _context.PolkVehicleYmmes.Where(x => x.VinPatternMask.ToLower() == vinPatternMask.ToLower()).FirstOrDefault();
                        if (polkVehicleYmmeObj != null)
                            break;
                    }
                }
                if (polkVehicleYmmeObj == null)
                {
                    if (stringList.Count < 1)
                        stringList.Add(vin);
                    throw new ApplicationException("The following VINs were tried and failed: " + string.Join(", ", (IEnumerable<string>)stringList));
                }
                DecodedPolkVehicle = polkVehicleYmmeObj != null ? (PolkVehicleYmme)polkVehicleYmmeObj : (PolkVehicleYmme)null;
                return DecodedPolkVehicle;
            }
        }
        public bool IsVinValid(string vin)
        {
            bool flag = true;
            if (vin.Length == 17)
            {
                string upper = vin.Substring(8, 1).ToUpper();
                int result = -1;
                if (upper == "X")
                    result = 10;
                else
                    int.TryParse(upper, out result);
                if (result > -1)
                {
                    int num = (this.GetVinCharacterChecksumValue(vin.Substring(0, 1), 1) + this.GetVinCharacterChecksumValue(vin.Substring(1, 1), 2) + this.GetVinCharacterChecksumValue(vin.Substring(2, 1), 3) + this.GetVinCharacterChecksumValue(vin.Substring(3, 1), 4) + this.GetVinCharacterChecksumValue(vin.Substring(4, 1), 5) + this.GetVinCharacterChecksumValue(vin.Substring(5, 1), 6) + this.GetVinCharacterChecksumValue(vin.Substring(6, 1), 7) + this.GetVinCharacterChecksumValue(vin.Substring(7, 1), 8) + this.GetVinCharacterChecksumValue(vin.Substring(9, 1), 10) + this.GetVinCharacterChecksumValue(vin.Substring(10, 1), 11) + this.GetVinCharacterChecksumValue(vin.Substring(11, 1), 12) + this.GetVinCharacterChecksumValue(vin.Substring(12, 1), 13) + this.GetVinCharacterChecksumValue(vin.Substring(13, 1), 14) + this.GetVinCharacterChecksumValue(vin.Substring(14, 1), 15) + this.GetVinCharacterChecksumValue(vin.Substring(15, 1), 16) + this.GetVinCharacterChecksumValue(vin.Substring(16, 1), 17)) % 11;
                    flag = result == num;
                }
                else
                    flag = false;
            }
            return flag;
        }
        private int GetVinCharacterChecksumValue(string c, int position)
        {
            int result = -1;
            int num = -1;
            int.TryParse(c, out result);
            if (result > -1)
            {
                switch (c.ToUpper())
                {
                    case "A":
                    case "J":
                        result = 1;
                        break;
                    case "B":
                    case "K":
                    case "S":
                        result = 2;
                        break;
                    case "C":
                    case "L":
                    case "T":
                        result = 3;
                        break;
                    case "D":
                    case "M":
                    case "U":
                        result = 4;
                        break;
                    case "E":
                    case "N":
                    case "V":
                        result = 5;
                        break;
                    case "F":
                    case "W":
                        result = 6;
                        break;
                    case "G":
                    case "P":
                    case "X":
                        result = 7;
                        break;
                    case "H":
                    case "Y":
                        result = 8;
                        break;
                    case "R":
                    case "Z":
                        result = 9;
                        break;
                }
            }
            if (result > -1)
            {
                switch (position)
                {
                    case 1:
                    case 11:
                        num = result * 8;
                        break;
                    case 2:
                    case 12:
                        num = result * 7;
                        break;
                    case 3:
                    case 13:
                        num = result * 6;
                        break;
                    case 4:
                    case 14:
                        num = result * 5;
                        break;
                    case 5:
                    case 15:
                        num = result * 4;
                        break;
                    case 6:
                    case 16:
                        num = result * 3;
                        break;
                    case 7:
                    case 17:
                        num = result * 2;
                        break;
                    case 8:
                        num = result * 10;
                        break;
                    case 10:
                        num = result * 9;
                        break;
                }
            }
            return num;
        }
        public bool IsVinAValidMaskPattern(string vin)
        {
            var obj = (object)null;
            var flag = false;
            switch (vin.Length)
            {
                case 4:
                case 5:
                case 6:
                    if (vin.IndexOf("*") > 0) flag = true;
                    break;
                case 7:
                    if (vin.IndexOf("*") > 0 || vin == vin.Substring(0, 3) + "****") flag = true;
                    break;
                case 8:
                    if (vin.IndexOf("*") > 0 || vin == vin.Substring(0, 3) + "*****") flag = true;
                    break;
                case 10:
                    if (vin == vin.Substring(0, 4) + "******" ||
                        vin == vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******") flag = true;
                    break;
                case 11:
                    if (vin == vin.Substring(0, 8) + "*" + vin.Substring(9, 2) + "******" ||
                        vin == vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******" ||
                        vin == vin.Substring(0, 5) + "******") flag = true;
                    break;
                case 12:
                    if (vin == vin.Substring(0, 6) + "******" || vin == vin.Substring(0, 5) + "*******")
                    {
                        flag = true;
                    }

                    break;
                case 13:
                    if (vin == vin.Substring(0, 7) + "******" || vin == vin.Substring(0, 6) + "*******") flag = true;
                    break;
                case 14:
                    if (vin == vin.Substring(0, 8) + "******" || vin == vin.Substring(0, 6) + "********") flag = true;
                    break;
                case 17:
                    var str = vin.Substring(0, 8);
                    if (vin == str + "*" + vin.Substring(9, 5) + "***" ||
                        vin == str + "*" + vin.Substring(9, 4) + "****" ||
                        vin == str + "*" + vin.Substring(9, 3) + "*****" ||
                        vin == str + "*" + vin.Substring(9, 2) + "******" ||
                        vin == str + "*" + vin.Substring(9, 1) + "*******" ||
                        vin == str + vin.Substring(8, 5) + "****" || vin == str + vin.Substring(8, 4) + "*****" ||
                        vin == str + vin.Substring(8, 3) + "******" ||
                        vin == str + vin.Substring(8, 2) + "*******") flag = true;
                    break;
            }
            return flag;
        }
        public List<string> GenerateVinPatternMasks(string vin)
        {
            var stringList = new List<string>();
            switch (vin.Length)
            {
                case 7:
                    stringList.Add(vin.Substring(0, 3) + "****");
                    break;
                case 8:
                    stringList.Add(vin.Substring(0, 3) + "*****");
                    break;
                case 10:
                    stringList.Add(vin.Substring(0, 4) + "******");
                    stringList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******");
                    break;
                case 11:
                    stringList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 2) + "******");
                    stringList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******");
                    stringList.Add(vin.Substring(0, 5) + "******");
                    break;
                case 12:
                    stringList.Add(vin.Substring(0, 6) + "******");
                    stringList.Add(vin.Substring(0, 5) + "*******");
                    break;
                case 13:
                    stringList.Add(vin.Substring(0, 7) + "******");
                    stringList.Add(vin.Substring(0, 6) + "*******");
                    break;
                case 14:
                    stringList.Add(vin.Substring(0, 8) + "******");
                    stringList.Add(vin.Substring(0, 6) + "********");
                    break;
                case 17:
                    var str = vin.Substring(0, 8);
                    stringList.Add(str + "*" + vin.Substring(9, 5) + "***");
                    stringList.Add(str + "*" + vin.Substring(9, 4) + "****");
                    stringList.Add(str + "*" + vin.Substring(9, 3) + "*****");
                    stringList.Add(str + "*" + vin.Substring(9, 2) + "******");
                    stringList.Add(str + "*" + vin.Substring(9, 1) + "*******");
                    break;
            }

            return stringList;
        }
        public VehicleInfoModel vaildatekey(string key)
        {
            var v = new VehicleInfoModel();

            if (ValidateKeyAndLogTransaction(key) == null)
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "000";
                v.ValidationFailures[0].Description = "Invalid Key";
                return v;
            }

            return v;

        }




        public int? GetTSBCountByVehicleCount(string AAIA)
        {
            var count = 0;
            using (innovaEntities dbContext = new innovaEntities())
            {
                var vehicleId = Convert.ToInt32(AAIA);

                count = (from tsb in dbContext.TSBs
                         join tv in dbContext.TSBToVehicles on tsb.TSBID equals tv.TSBID

                         join tlb in dbContext.TSBAAIAToLegacyVehicleIDs on tv.VehicleId equals tlb.LegacyVehicleID
                         where tlb.AAIAVehicleID == vehicleId

                         select tsb.TSBID).Distinct().Count();

                if (count != 0)
                    return count;
                return count;
            };
        }
        public int GetRecallsCountForVehicleByYearMakeModel(int year, string make, string model, string trimLevel, string startDateString, string endDateString)
        {

           RecallCollection recalls = null;

            if (String.IsNullOrEmpty(startDateString) && String.IsNullOrEmpty(endDateString))
            {
                var Count = RecallWebService.Search(year, make, model, trimLevel);
                return Count;
            }
            else
            {
                recalls = RecallWebService.GetByYearMakeModelDateRange(year, make, model, startDateString, endDateString);
            }
            return recalls.Count;
        }
        public DiagReportInfo LogDiagnosticReportWithMileage(ApiRequestModel apiRequest)
        {
            var drInfo = new DiagReportInfo();
            Guid? id = ValidateKey(apiRequest.key);
            if (id.HasValue)
            {
                if (DiagnosticReportWebService.IsDuplicate(id, apiRequest))
                {
                    if (apiRequest.vehicleMileage == 0)
                    {
                                using (innovaEntities dbContext = new innovaEntities())
                                {
                               var polkVehicleYMME = dbContext.PolkVehicleYmmes.Where(x => x.VinPatternMask == apiRequest.vin).FirstOrDefault();
                                    if (polkVehicleYMME != null)
                                    {
                                        int vehicleYear = Convert.ToInt32(polkVehicleYMME.Year);
                                        int currentYear = DateTime.Now.Year;
                                        int currentMonth = DateTime.Now.Month;
                                        int vehicleAgeInMonths = ((currentYear - vehicleYear) * 12) + currentMonth;
                                        apiRequest.vehicleMileage = vehicleAgeInMonths * 1000;
                                        return DiagnosticReportWebService.GetDiagnosticReport(apiRequest); ;
                                    }
                                };
                    }
                }
                else
                {
                    drInfo.ValidationFailures = new ValidationFailureModel[1];
                    drInfo.ValidationFailures[0] = new ValidationFailureModel();
                    drInfo.ValidationFailures[0].Code = "420";
                    drInfo.ValidationFailures[0].Description = "Duplicate key";
                    drInfo.IsValid = false;
                }
            }
            return drInfo;
        }
        public DiagReportInfo GetMileage(ApiRequestModel apiRequest)
        {
            var drInfo = new DiagReportInfo();
            Guid? id = ValidateKey(apiRequest.key);
            if (id.HasValue)
            {
                if (DiagnosticReportWebService.IsDuplicate(id, apiRequest))
                {
                    if (apiRequest.vehicleMileage != 0)
                    {
                        using (innovaEntities dbContext = new innovaEntities())
                        {
                            var polkVehicleYMME = dbContext.PolkVehicleYmmes.Where(x => x.VinPatternMask == apiRequest.vin).FirstOrDefault();
                            if (polkVehicleYMME != null)
                            {
                                return DiagnosticReportWebService.GetMileage(apiRequest); ;
                            }
                        };
                    }
                }
                else
                {
                    drInfo.ValidationFailures = new ValidationFailureModel[1];
                    drInfo.ValidationFailures[0] = new ValidationFailureModel();
                    drInfo.ValidationFailures[0].Code = "420";
                    drInfo.ValidationFailures[0].Description = "Duplicate key";
                    drInfo.IsValid = false;
                }
            }
            return drInfo;
        }
        public DTCLaymensTermInfo GetByDtcAndMake(RequestModel apiRequest)
        {
            var drInfo = new DTCLaymensTermInfo();

            if (!string.IsNullOrEmpty(apiRequest.make) && !string.IsNullOrEmpty(apiRequest.dtcCode) )
            {

                using (innovaEntities dbContext = new innovaEntities())
                {
                    var vinDecoder = (from dTCCodeLaymanTerms in dbContext.DTCCodeLaymanTerms
                                      join dTCCodeLaymanTermSeverityDefinitions in dbContext.DTCCodeLaymanTermSeverityDefinitions
                                      on dTCCodeLaymanTerms.SeverityLevel equals dTCCodeLaymanTermSeverityDefinitions.SeverityLevel
                                      where dTCCodeLaymanTerms.ErrorCode != null && dTCCodeLaymanTerms.ErrorCode.ToLower() == apiRequest.dtcCode.ToLower()
                                      && dTCCodeLaymanTerms.Make != null && dTCCodeLaymanTerms.Make.ToLower() == apiRequest.make.ToLower()
                                      select new DTCLaymensTermInfo {
                                          ErrorCode = dTCCodeLaymanTerms.ErrorCode,
                                          Make = dTCCodeLaymanTerms.Make,
                                          Title = dTCCodeLaymanTerms.Title,
                                          DTCCodeLaymanTermId = Convert.ToInt32( dTCCodeLaymanTerms.DTCCodeLaymanTermId),
                                          Description = dTCCodeLaymanTerms.Description,
                                          SeverityLevel = Convert.ToInt32(dTCCodeLaymanTerms.SeverityLevel),
                                          SeverityLevelDefinition = dTCCodeLaymanTermSeverityDefinitions.SeverityDefinition,
                                          EffectOnVehicle = dTCCodeLaymanTerms.EffectOnVehicle,
                                          ResponsibleComponentOrSystem = dTCCodeLaymanTerms.ResponsibleComponentOrSystem,
                                          WhyItsImportant = dTCCodeLaymanTerms.WhyItsImportant,
                                          IsValid = true
                                      }).Distinct().OrderByDescending(x => x.Make).FirstOrDefault();

                    if(vinDecoder == null)
                    {
                        vinDecoder.ValidationFailures = new ValidationFailureModel[1];
                        vinDecoder.ValidationFailures[0] = new ValidationFailureModel();
                        vinDecoder.ValidationFailures[0].Code = "210";
                        vinDecoder.ValidationFailures[0].Description = "Data not found";
                        vinDecoder.IsValid = false;
                    }
                    return vinDecoder;
                };
            }
            else
            {
                drInfo.ValidationFailures = new ValidationFailureModel[1];
                drInfo.ValidationFailures[0] = new ValidationFailureModel();
                drInfo.ValidationFailures[0].Code = "404";
                drInfo.ValidationFailures[0].Description = "make or dtc code is null!";
                drInfo.IsValid = false;
            }
            return drInfo;
        }
        public DLCLocationInfo GetDLCLocation(VehicleRequest apiRequest)
        {
            var wsObject = new DLCLocationInfo();
            var year = Convert.ToString(apiRequest.Year);
            if (!string.IsNullOrEmpty(year)   && !string.IsNullOrEmpty(apiRequest.Make) && !string.IsNullOrEmpty(apiRequest.Model))
            {
                using (innovaEntities dbContext = new innovaEntities())
                {
                    var sdkObject = dbContext.DLCLocations.Where(x => x.Make.ToLower() == apiRequest.Make.ToLower() && x.Model.ToLower() == apiRequest.Model.ToLower() && x.Year.ToLower() == year.ToLower()).Distinct().FirstOrDefault();
                    if (sdkObject != null)
                    {
                        var AWSPrivateKeyPath = GlobalModel.AmazonWebServicePrivateKeyPath;
                        var AWSPrivateKey = GlobalModel.AmazonWebServicePrivateKey;
                        var AWSSecureDomain = GlobalModel.DlcImageRootUrl;
                        var fullImagePath = AWSSecureDomain + "/";                //sdkObject.GetImageRelativeUrl(externalSystem, false);
                        var fullImagePathThumbnail = AWSSecureDomain + "/";      //+ sdkObject.GetImageRelativeUrl(externalSystem, true);
                        wsObject.Year = sdkObject.Year;
                        wsObject.Make = sdkObject.Make;
                        wsObject.Model = sdkObject.Model;
                        wsObject.LocationNumber = sdkObject.LocationNumber;
                        wsObject.Access = sdkObject.Access;
                        wsObject.Comments = sdkObject.Comments;
                        wsObject.ImageFileName = sdkObject.ImageFileName;
                        wsObject.IsValid = true;
                        wsObject.ImageFileUrl = "https://secure-downloads.innova.com/dlc-location-images-wm/hyundai/sonata/sonata-2-cmd.jpg?Expires=1511017878&amp;Signature=mvaNUzN-w4ZOJKVR99kyasS87aDx7Fi~NoBsvNKiAdcQh6jdFoP567VO0EB0dsHPlOL94UFs52xhThW6LeZp4Z-pJ0fgGtY5esj3JK22pY25yXhBmHtFH4JRoQiO~xjcGIkB-EKFhdtznyH14hwkE1veustzDvefio7R35dHjJi5H~Q3l3TOF-0-8TIrv2VXlonLNLavYhpqzCXDYGS1K97g5C7oYjm3u1MAUhwt6g4AVdqicO7QAeWk0U6H8gtlqnhPmVgd-3JfMHeNnc4x8NA1ESMQaQM7mtnWGU9QDuZcRjyW7ensMKAouwuy7Rae9pb1IbA~GH1V8L7GpA74Iw__&amp;Key-Pair-Id=" + AWSPrivateKey;
                                                 // GetTempUrlSigned(fullImagePath, AWSPrivateKeyPath, AWSPrivateKey);
                        wsObject.ImageFileUrlSmall = "https://secure-downloads.innova.com/dlc-location-images-wm/hyundai/sonata/sonata-2-cmdsm.jpg?Expires=1511017878&amp;Signature=H~Dm1NdCrAhpv9vOJ7WlSSdZbZICne4G1GvSNetfObmF255CxbdMOHihggu0au8oe4ED2ohee64XZdcwrN-gKhEOUGqb0W8Zg8fjfDyGl5yG8388X56Z8qSfMY0pfnPj1c6wSeQ2JLtLoh6p4hsfhUvvN3xkH8-S4jg7PCowuqNHeSagF85TyOO1edHoZz7Brmj-23jV6~Rd8CBj3HQhyMYnT5hXiEcsD7BYor0GnsZU~ioZLFDoTEzJWVduL8AKZm9rl09ioUIuUu~aNc3Eb21a1ftKlGokhj1317SbPIE1sh6Mnr8DCbp1MFT2NGEOkZfLNalRnAE9OHuP4xtLHA__&amp;Key-Pair-Id=" + AWSPrivateKey;
                                                //GetTempUrlSigned(fullImagePathThumbnail, AWSPrivateKeyPath, AWSPrivateKey);
                        return wsObject;
                    }
                    else
                    {
                        wsObject.ValidationFailures = new ValidationFailureModel[1];
                        wsObject.ValidationFailures[0] = new ValidationFailureModel();
                        wsObject.ValidationFailures[0].Code = "404";
                        wsObject.ValidationFailures[0].Description = "Data not found!";
                        wsObject.IsValid = false;
                    }
                };
            }

            else
            {
                wsObject.ValidationFailures = new ValidationFailureModel[1];
                wsObject.ValidationFailures[0] = new ValidationFailureModel();
                wsObject.ValidationFailures[0].Code = "400";
                wsObject.ValidationFailures[0].Description = "Make or Model or Year value is null !";
                wsObject.IsValid = false;
            }

            return wsObject;
        }
        public static string GetTempUrlSigned(string urlString, string AWSPrivateKeyPath, string AWSPrivateKey)
        {
            string durationUnits = "minutes";
            string durationNumber = "5";
            string privateKeyId = AWSPrivateKey;
            string pathToPrivateKey = AWSPrivateKeyPath;
            return CreateCannedPrivateURL(urlString, durationUnits, durationNumber, pathToPrivateKey, privateKeyId);
        }
        public static string CreateCannedPrivateURL(string urlString, string durationUnits, string durationNumber, string pathToPrivateKey, string privateKeyId)
        {
            TimeSpan duration = GetDuration(durationUnits, durationNumber);
            string policyStatement = CreatePolicyStatement(urlString, DateTime.UtcNow.Add(duration), "0.0.0.0/0");
            if ("Error!" == policyStatement)
                return "Invalid time frame. Start time cannot be greater than end time.";
            string str = CopyExpirationTimeFromPolicy(policyStatement);
            byte[] bytes = Encoding.ASCII.GetBytes(policyStatement);
            using (SHA1CryptoServiceProvider cryptoServiceProvider1 = new SHA1CryptoServiceProvider())
            {
                byte[] hash = cryptoServiceProvider1.ComputeHash(bytes);
                RSACryptoServiceProvider cryptoServiceProvider2 = new RSACryptoServiceProvider();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pathToPrivateKey);
                cryptoServiceProvider2.FromXmlString(xmlDocument.InnerXml);
                RSAPKCS1SignatureFormatter signatureFormatter = new RSAPKCS1SignatureFormatter((AsymmetricAlgorithm)cryptoServiceProvider2);
                signatureFormatter.SetHashAlgorithm("SHA1");
                string safeBase64String = ToUrlSafeBase64String(signatureFormatter.CreateSignature(hash));
                return urlString + "?Expires=" + str + "&Signature=" + safeBase64String + "&Key-Pair-Id=" + privateKeyId;
            }
        }
        public static TimeSpan GetDuration(string units, string numUnits)
        {
            TimeSpan timeSpan = new TimeSpan();
            if (!(units == "seconds"))
            {
                if (!(units == "minutes"))
                {
                    if (!(units == "hours"))
                    {
                        if (units == "days")
                            timeSpan = new TimeSpan(int.Parse(numUnits), 0, 0, 0);
                        else
                            Console.WriteLine("Invalid time units; use seconds, minutes, hours, or days");
                    }
                    else
                        timeSpan = new TimeSpan(0, int.Parse(numUnits), 0, 0);
                }
                else
                    timeSpan = new TimeSpan(0, 0, int.Parse(numUnits), 0);
            }
            else
                timeSpan = new TimeSpan(0, 0, 0, int.Parse(numUnits));
            return timeSpan;
        }
        public static string CreatePolicyStatement(string resourceUrl, DateTime endTime, string ipAddress)
        {
            string str = "{\"Statement\":[{\"Resource\":\"RESOURCE_FILL\",\"Condition\":{\"DateLessThan\":{\"AWS:EpochTime\":EXPIRES_FILL}}}]}";
            int timestamp = (int)ConvertToTimestamp(endTime);
            return Regex.Replace(str.Replace("RESOURCE_FILL", resourceUrl).Replace("EXPIRES_FILL", timestamp.ToString()), "\\s+", "");
        }
        private static double ConvertToTimestamp(DateTime value)
        {
            return (value - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
        public static string CopyExpirationTimeFromPolicy(string policyStatement)
        {
            int num = policyStatement.IndexOf("EpochTime");
            string str = policyStatement.Substring(num + "EpochTime".Length);
            List<char> charList = new List<char>((IEnumerable<char>)new char[10]
            {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9'
            });
            StringBuilder stringBuilder = new StringBuilder(20);
            foreach (char ch in str)
            {
                if (charList.Contains(ch))
                    stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }
        public static string ToUrlSafeBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('=', '_').Replace('/', '~');
        }

        public virtual List<ScheduleMaintenanceServiceInfo> GetScheduledMaintenanceNextServiceForVehicle(int years, string manufacturers, string makes, string models, string enginetype, string trimlevel, string transmission, string enginevincode, int currentmileage)
        {
            if (currentmileage == 0)
            {
                return new List<ScheduleMaintenanceServiceInfo>();
            }
            DataBase.Vehicle request = new DataBase.Vehicle();
            request.Year = years;
            request.ManufacturerName = manufacturers;
            request.Make = makes;
            request.Model = models;
            request.EngineType = enginetype;
            request.TrimLevel = trimlevel;
            request.TransmissionControlType = transmission;
            request.EngineVINCode = enginevincode;

            int? vehicleYear = null;
            ScheduleMaintenancePlan plan = null;

            if (request != null)
            {
                vehicleYear = request.Year;
                plan = GetScheduledMaintenanceNextServiceAsyc(request);

            }
            if (plan != null)
            {
                List<ScheduleMaintenanceServiceInfo> scheduleMaintenanceServiceInfos = GetNextServices(request, plan, true, DateTime.UtcNow, 0, false);
                return scheduleMaintenanceServiceInfos;
            }
            return new List<ScheduleMaintenanceServiceInfo>();
        }

        public virtual ScheduleMaintenancePlan GetScheduledMaintenanceNextServiceAsyc(DataBase.Vehicle vehicle)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.ScheduleMaintenancePlans
                     .GroupJoin(_context.ScheduleMaintenancePlanEngineTypes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanEngineType => ScheduleMaintenancePlanEngineType.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineType) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineType })
                     .GroupJoin(_context.ScheduleMaintenancePlanEngineVINCodes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanEngineVINCode => ScheduleMaintenancePlanEngineVINCode.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineVINCode) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineVINCode })
                     .GroupJoin(_context.ScheduleMaintenancePlanMakes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanMake => ScheduleMaintenancePlanMake.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanMake) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanMake })
                     .GroupJoin(_context.ScheduleMaintenancePlanModels.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanModel => ScheduleMaintenancePlanModel.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanModel) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanModel })
                     .GroupJoin(_context.ScheduleMaintenancePlanYears.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanYear => ScheduleMaintenancePlanYear.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanYear) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanYear })
                     .GroupJoin(_context.ScheduleMaintenancePlanTrimLevels.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanTrimLevel => ScheduleMaintenancePlanTrimLevel.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanTrimLevel) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanTrimLevel })
                     .GroupJoin(_context.ScheduleMaintenanceTransmissions.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanTransmission => ScheduleMaintenancePlanTransmission.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanTransmission) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanTransmission })
                     .Where(e =>
                        (vehicle.Year == 0 || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanYear.Any(x => x.Year == vehicle.Year))                                                                                  //filter by YearsString
                        && (vehicle.Make == null || vehicle.Make.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanMake.Any(x => x.Make == vehicle.Make)                                           //filter by MakesString 
                        && (vehicle.Model == null || vehicle.Model.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanModel.Any(x => x.Model == vehicle.Model)                                       //filter by ModelsString 
                        && (vehicle.EngineType == null || vehicle.EngineType.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanEngineType.Any(x => x.EngineType == vehicle.EngineType)                   //filter by EngineTypesString 
                        && (vehicle.TrimLevel == null || vehicle.TrimLevel.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlanTrimLevel.Any(x => x.TrimLevel == vehicle.TrimLevel)                   //filter by TrimLevelsString 
                          && (vehicle.EngineVINCode == null || vehicle.EngineVINCode.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanEngineVINCode.Any(x => x.EngineVINCode == vehicle.EngineVINCode)
                        && (vehicle.TransmissionControlType == null || vehicle.TransmissionControlType.Length == 0) || e.ScheduleMaintenancePlanTransmission.Any(x => x.Transmission == vehicle.TransmissionControlType)
                        && e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.Type == 0)           //filter by TransmissionsString 
                     .FirstOrDefault()?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan;
            };
        }
        public virtual List<ScheduleMaintenanceServiceInfo> GetNextServices(Vehicle vehicleInfo, ScheduleMaintenancePlan planInfo, bool lookupFixesPartsAndLabor, DateTime today, int milesDrivenPerDay, bool useSingleDayMileageWindow)
        {
            int mileage = GetEstimatedMileage(vehicleInfo, today, milesDrivenPerDay);
            List<ScheduleMaintenancePlanDetail> planDetails = GetByPlanId(planInfo.ScheduleMaintenancePlanId);
            var PlanDetails = new List<ScheduleMaintenancePlanDetail>();
            var nextServicesPlanDetails = new List<ScheduleMaintenancePlanDetail>();

            int nextMileageInterval = 0;
            foreach (ScheduleMaintenancePlanDetail plan in planDetails)
            {
                int mileageMax = 9999999;
                nextMileageInterval = GetNextServiceMileageInterval(mileage, Convert.ToInt32( plan.Mileage), Convert.ToInt32(plan.MileageRepeat));
                if (nextMileageInterval >= mileage && nextMileageInterval < mileageMax
                    || nextMileageInterval >= mileage && mileageMax == 0)
                {
                    if (planInfo.Type == 0 || (mileage + 15000 >= nextMileageInterval))
                    {
                        PlanDetails.Add(plan);
                    }
                }
            }
            int planNextMileage = 0;
            nextMileageInterval = 0;
            foreach (ScheduleMaintenancePlanDetail plan in PlanDetails)
            {
                planNextMileage = GetNextServiceMileageInterval(mileage, Convert.ToInt32(plan.Mileage), Convert.ToInt32(plan.MileageRepeat));
                if (nextMileageInterval == 0 || planNextMileage < nextMileageInterval)
                {
                    nextMileageInterval = planNextMileage;
                }
            }

            foreach (ScheduleMaintenancePlanDetail plan in planDetails)
            {
                if (GetNextServiceMileageInterval(mileage, Convert.ToInt32( plan.Mileage), Convert.ToInt32(plan.MileageRepeat)) == nextMileageInterval)
                {
                    nextServicesPlanDetails.Add(plan);
                }
            }
            List<ScheduleMaintenanceServiceInfo> services = new List<ScheduleMaintenanceServiceInfo>();
            if (nextServicesPlanDetails != null && nextServicesPlanDetails.Count > 0)
            {
                for (int i = 0; i < planDetails.Count; i++)
                {
                    services.Add(GetWebServiceObject(planDetails[i], mileage));
                }
            }
            return services.ToList();
        }
        public int GetEstimatedMileage(DataBase.Vehicle vehicleInfo, DateTime date, int milesDrivenPerDay)
        {
            if (vehicleInfo.Mileage == 0)
            {
                vehicleInfo.Mileage = GetLastVehicleMileageByUserIdVehicleId(vehicleInfo);
                vehicleInfo.MileageLastRecordedDateTimeUTC = DateTime.UtcNow;
            }

            if (vehicleInfo.MileageLastRecordedDateTimeUTC == null)
            {
                return milesDrivenPerDay;
            }

            TimeSpan ts = date.Subtract(Convert.ToDateTime(vehicleInfo.MileageLastRecordedDateTimeUTC));
            int daysSinceLastMileage = (int)ts.TotalDays;

            return Convert.ToInt32(vehicleInfo.Mileage) + (daysSinceLastMileage * milesDrivenPerDay);
        }
        public int GetLastVehicleMileageByUserIdVehicleId(Vehicle info)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.DiagnosticReports.Where(e => info.VehicleId == e.VehicleId && info.UserId == e.UserId).Select(x => x.VehicleMileage).FirstOrDefault();
            };
        }
        public List<ScheduleMaintenancePlanDetail> GetByPlanId(string planId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return (from s in _context.ScheduleMaintenancePlans
                        join b in _context.ScheduleMaintenancePlanDetails
                        on s.ScheduleMaintenancePlanId equals b.ScheduleMaintenancePlanId
                        where s.ScheduleMaintenancePlanId == planId
                        select b).ToList();
            }
        }
        public int GetNextServiceMileageInterval(int Mileage, int MileageRepeat, int currentMileage)
        {
            int NextServiceMileageInterval;
            if (currentMileage <= Mileage)
            {
                NextServiceMileageInterval = Mileage;
                return Mileage;
            }
            if (MileageRepeat <= 0)
            {
                NextServiceMileageInterval = 0;
                return 0;
            }
            currentMileage = currentMileage - Mileage;
            decimal intervals = (decimal)((decimal)currentMileage / (decimal)MileageRepeat);
            if (intervals > decimal.Floor(intervals))
            {
                intervals = decimal.Floor(intervals) + 1;
            }
            NextServiceMileageInterval = (MileageRepeat * (int)intervals) + Mileage;
            return NextServiceMileageInterval;
        }
        public ScheduleMaintenanceServiceInfo GetWebServiceObject(ScheduleMaintenancePlanDetail sdkObject, int mileage)
        {
            ScheduleMaintenanceServiceInfo wsObject = new ScheduleMaintenanceServiceInfo();
            var fix = GetFixByServiceId(sdkObject.ScheduleMaintenanceServiceId);
            var fixName = GetByFixNameId(fix.FixNameId);

            wsObject.Name = fixName?.Description;
            wsObject.Category = "";
            wsObject.Mileage = GetNextServiceMileageInterval(mileage, Convert.ToInt32(sdkObject.Mileage), Convert.ToInt32(sdkObject.MileageRepeat));
            if (fixName != null)
            {
                wsObject.ServiceInfo = GetWebServiceObjectFixInfo(fix, fixName, sdkObject);
            }
            return wsObject;
        }

        public Fix GetFixByServiceId(string serviceId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.ScheduleMaintenanceServices
                         .Join(_context.Fixes, scheduleMaintenanceService => scheduleMaintenanceService.FixNameId, B => B.FixNameId, (scheduleMaintenanceService, B) => new { scheduleMaintenanceService, B })
                         .Where(e => e.scheduleMaintenanceService.ScheduleMaintenanceServiceId.Equals(serviceId)).Select(e => e.B).FirstOrDefault();
            }
        }
        public FixName GetByFixNameId(string fixNameId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.FixNames.Where(x => x.FixNameId == fixNameId).FirstOrDefault();
            }
        }

        public FixInfo GetWebServiceObjectFixInfo(Fix fix, FixName fixName, ScheduleMaintenancePlanDetail sdkObject)
        {
            FixInfo wsObject = new FixInfo();
            if (fixName != null)
            {
                wsObject.FixNameId = fixName.FixNameId;
            }
            wsObject.FixId = fix.FixId;
            wsObject.Name = fixName.Description;
            wsObject.Description = fixName.Description;
            wsObject.LaborHours = fix.Labor;

            var fixParts = GetByFixId(fix.FixId);
            ReCalculateFixInfoCosts(wsObject, fix, fixParts);
            wsObject.FixRating = (int)fixName.FixRating;


            var fixFeedbacks = new List<DiagnosticReportFixFeedback>();
            var fixDTCs = GetFixDTCByFixId(fix.FixId);

            if (fixDTCs != null && fixDTCs.Count > 0)
            {
                var PrimaryDTC = ((FixDTC)fixDTCs[0]).PrimaryDTC;
                fixFeedbacks = GetFixFeedbacksByPrimaryDTC(PrimaryDTC, fix);
                wsObject.FixFeedbacks = new FixFeedbackInfo[fixFeedbacks.Count];

                for (int i = 0; i < fixFeedbacks.Count; i++)
                {
                    FixFeedbackInfo wsFixFeedObject = new FixFeedbackInfo();

                    wsFixFeedObject.DiagnosticReportId =Convert.ToInt32( fixFeedbacks[i].DiagnosticReportId);
                    wsFixFeedObject.IsReportValid = Convert.ToBoolean( fixFeedbacks[i].IsReportValid);
                    wsFixFeedObject.CouldNotFixReason = fixFeedbacks[i].CouldNotFixReason;
                    wsFixFeedObject.PrimaryErrorCode = fixFeedbacks[i].PrimaryErrorCode;
                    wsFixFeedObject.DiagnosticReportErrorCodeSystemType = Enumerations.GetEnumDescription((DiagnosticReportErrorCodeSystemType)fixFeedbacks[i].DiagnosticReportErrorCodeSystemType);
                    if (fixName != null)
                    {
                        wsFixFeedObject.Fix = fixName.Description;
                    }
                    wsFixFeedObject.AverageDiagnosticTimeMinutes = Convert.ToInt32(fixFeedbacks[i].AverageDiagnosticTime);
                    wsFixFeedObject.FrequencyEncountered = Convert.ToInt32(fixFeedbacks[i].FrequencyEncountered);
                    wsFixFeedObject.FixDifficultyRating = Enumerations.GetEnumDescription((FixDifficultyRating)fixFeedbacks[i].FixDifficultyRating);
                    wsFixFeedObject.ErrorCodesThatApply = fixFeedbacks[i].ErrorCodesThatApply;
                    wsFixFeedObject.TechComments = fixFeedbacks[i].TechComments;
                    wsFixFeedObject.BasicToolsRequired = fixFeedbacks[i].BasicToolsRequired;
                    wsFixFeedObject.SpecialtyToolsRequired = fixFeedbacks[i].SpecialtyToolsRequired;
                    wsFixFeedObject.TipsAndTricks = fixFeedbacks[i].TipsAndTricks;
                    List<DiagnosticReportFixFeedbackPart> feedbackParts = GetFeedbackPartsByDiagnosticReportFixFeedbackId(fixFeedbacks[i].DiagnosticReportFixFeedbackId);
                    wsFixFeedObject.Parts = new FixFeedbackPartInfo[feedbackParts.Count];

                    for (int j = 0; j < feedbackParts.Count; j++)
                    {
                        FixFeedbackPartInfo wsFixFeedbackPartObject = new FixFeedbackPartInfo();

                        if (feedbackParts[j].DiagnosticReportFixFeedbackId != null)
                        {
                            wsFixFeedbackPartObject.DiagnosticReportFixFeedbackId = Guid.Parse(feedbackParts[j].DiagnosticReportFixFeedbackId);
                        }
                        wsFixFeedbackPartObject.PartName = feedbackParts[j].PartName;
                        wsFixFeedbackPartObject.PartNumber = feedbackParts[j].PartNumber;
                        wsFixFeedObject.Parts[j] = wsFixFeedbackPartObject;
                    }
                    wsObject.FixFeedbacks[i] = wsFixFeedObject;
                }
            }

            wsObject.FixParts = new FixPartInfo[fixParts.Count];

            for (int i = 0; i < fixParts.Count; i++)
            {
                FixPartInfo wsFixPartObject = new FixPartInfo();
                Part part = GetPartByPartId(fixParts[i].PartId);
                PartName partName = GetPartNameByPartNameId(part.PartNameId);
                wsFixPartObject.ACESPartTypeID = partName.ACESId;
                wsFixPartObject.Quantity = Convert.ToInt32( fixParts[i].Quantity);
                wsFixPartObject.Name = partName.Name;
                wsFixPartObject.Description = partName.Name;

                wsFixPartObject.ManufacturerName = "";
                wsFixPartObject.MakesList = part.MakesString;
                wsFixPartObject.PartNumber = part.PartNumber;
                wsFixPartObject.Price =Convert.ToDecimal(  part.Price);
                wsFixPartObject.CodemasterID = fixParts[i].CodemasterID;
                wsObject.FixParts[i] = wsFixPartObject;
            }
            return wsObject;
        }
        public static class Enumerations
        {
            public static string GetEnumDescription(Enum value)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
        }
        public List<FixPart> GetByFixId(string fixId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.FixParts.Where(x => x.FixId == fixId).ToList();
            }
        }

        public void ReCalculateFixInfoCosts(FixInfo wsObject, Fix fix, List<FixPart> fixParts)
        {
            wsObject.LaborHours = fix.Labor;
            decimal rate = 0;
            var stateLaborRate = GetStateLaborRateByStateCode(StateCode);
            if (stateLaborRate != null)
            {
                rate = Convert.ToDecimal( stateLaborRate.DollarsPerHour);
            }
            wsObject.LaborRate = rate;
            wsObject.LaborCost = fix.Labor * wsObject.LaborRate;
            wsObject.AdditionalCost = fix.AdditionalCost;
            wsObject.PartsCost = 0;
            foreach (FixPart fixPart in fixParts)
            {
                var part = GetPartByPartId(fixPart.PartId);
                wsObject.PartsCost += (fixPart.Quantity * part.Price);
            }
            wsObject.TotalCost = wsObject.LaborCost + wsObject.AdditionalCost + wsObject.PartsCost;
            if (currentCurrency != Currency.USD)
            {
                wsObject.LaborRate = GetLocalCurrencyValueFromUSDollars(Convert.ToDecimal(wsObject.LaborRate));
                wsObject.LaborCost = GetLocalCurrencyValueFromUSDollars(Convert.ToDecimal(wsObject.LaborCost));
                wsObject.AdditionalCost = GetLocalCurrencyValueFromUSDollars(Convert.ToDecimal(wsObject.AdditionalCost));
                wsObject.PartsCost = GetLocalCurrencyValueFromUSDollars(Convert.ToDecimal(wsObject.PartsCost));
                wsObject.TotalCost = GetLocalCurrencyValueFromUSDollars(Convert.ToDecimal(wsObject.TotalCost));
            }
        }

        public StateLaborRate GetStateLaborRateByStateCode(string stateCode)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.StateLaborRates
               .Where(e =>
                   (e.StateCode == stateCode))
                   .Distinct()
                   .FirstOrDefault();
            }
        }
        public Part GetPartByPartId(string PartId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.Parts.Where(x => x.PartId == PartId).FirstOrDefault();
            }
        }

        public Decimal GetLocalCurrencyValueFromUSDollars(Decimal usDollarsValue)
        {
            return this.ConvertCurrency(Currency.USD, usDollarsValue, currentCurrency);
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
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.CurrencyExchangeRates.Where(e => (e.CurrencyISOCode == CurrenctISOCode)) .Distinct().Select(x => x.ExchangeRatePerUSD).FirstOrDefault();
            }
        }
        public List<FixDTC> GetFixDTCByFixId(string FixId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.FixDTCs.Where(x => x.FixId.Equals(FixId)).ToList();
            }
        }

        public List<DiagnosticReportFixFeedback> GetFixFeedbacksByPrimaryDTC(string PrimaryDTC, Fix fix)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.DiagnosticReportFixFeedbacks
                              .Join(_context.ObdFixes, dr => dr.ObdFixId.DefaultIfEmpty(), obd => obd.ObdFixId, (dr, obd) => new { dr, obd })
                              .Where(e => e.dr.PrimaryErrorCode == PrimaryDTC
                               && string.IsNullOrEmpty(e.obd.FixId) && e.dr.FixId == fix.FixId
                               || !string.IsNullOrEmpty(e.obd.FixId) && e.obd.FixId == fix.FixId)
                              .Distinct()
                              .Select(x => x.dr).ToList();
            }
        }
        public List<DiagnosticReportFixFeedbackPart> GetFeedbackPartsByDiagnosticReportFixFeedbackId(string DiagnosticReportFixFeedbackId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.DiagnosticReportFixFeedbackParts.Where(x => x.DiagnosticReportFixFeedbackId == DiagnosticReportFixFeedbackId).ToList();
            }
        }


        public PartName GetPartNameByPartNameId(string PartNameId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                return _context.PartNames.Where(x => x.PartNameId == PartNameId).FirstOrDefault();
            }
        }
    }
}