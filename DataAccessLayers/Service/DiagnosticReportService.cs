using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayers.WebService;
using DataAccessLayers.Model;
using DataAccessLayers.DataObjects;
using System.Xml;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace DataAccessLayers.Service
{
    public class DiagnosticReportService
    {
        public Guid? ValidateKeyAndLogTransaction(string key)
        {
            var externalSystem = new ExternalSystemWebService();
            Guid? id = ValidateKey(key);
            if (id.HasValue)
            {
                // [dbo].[ExternalSystemTransaction_Create]  
            }
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
        public VehicleInfoModel GetVehicleInfo(string key, string vin)
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

            string errorMessage = "";
            try
            {
                if (vin != null && !IsVinAValidMaskPattern(vin))
                {
                    foreach (string vinPatternMask in GenerateVinPatternMasks(vin))
                    {
                        using (innovadev01Entities dbContext = new innovadev01Entities())
                        {
                            var vinDecoder = dbContext.PolkVehicleYMMEs.Where(x => x.VinPatternMask == vinPatternMask).FirstOrDefault();
                            if (vinDecoder != null)
                            {
                                v.VIN = vinDecoder.VinPatternMask;
                                v.ManufacturerName = vinDecoder.Manufacturer;
                                v.Make = vinDecoder.Make;
                                v.Year = vinDecoder.Year.ToString();
                                v.Model = vinDecoder.Model;
                                v.Transmission = vinDecoder.Transmission;
                                v.EngineType = vinDecoder.EngineType;
                                v.TrimLevel = vinDecoder.Trim;
                                v.EngineVINCode = vinDecoder.EngineVinCode;
                                v.AAIA = vinDecoder.AAIA;
                                v.ModelImageFileUrl = GlobalModel.PolkVehicleImageRootUrl;
                            }
                        };
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "There was an error decoding the vin: " + ex.Message;
            }

            if (vin == null  || IsVinAValidMaskPattern(vin))
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "426";
                v.ValidationFailures[0].Description = "The VIN supplied is invalid, you cannot retreive a diagnostic report without a valid vin";
                v.IsValid = false;
            }

            else if (errorMessage.Length > 0 || v == null)
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "420";
                v.ValidationFailures[0].Description = errorMessage;
                v.IsValid = false;
            }

            return v;

        }
        public VehicleInfoModel GetVehicleInfo(string key)
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
        public VehicleInfoModel GetVehicleInfoByVin(string vin)
        {
            var v = new VehicleInfoModel();
            string errorMessage = "";
            try
            {
                if (vin != null && !IsVinAValidMaskPattern(vin))
                {
                    foreach (string vinPatternMask in GenerateVinPatternMasks(vin))
                    {
                        using (innovadev01Entities dbContext = new innovadev01Entities())
                        {
                            var vinDecoder = dbContext.PolkVehicleYMMEs.Where(x => x.VinPatternMask == vinPatternMask).FirstOrDefault();
                            if (vinDecoder != null)
                            {
                                v.VIN = vinDecoder.VinPatternMask;
                                v.ManufacturerName = vinDecoder.Manufacturer;
                                v.Make = vinDecoder.Make;
                                v.Year = vinDecoder.Year.ToString();
                                v.Model = vinDecoder.Model;
                                v.Transmission = vinDecoder.Transmission;
                                v.EngineType = vinDecoder.EngineType;
                                v.TrimLevel = vinDecoder.Trim;
                                v.EngineVINCode = vinDecoder.EngineVinCode;
                                v.AAIA = vinDecoder.AAIA;
                                v.ModelImageFileUrl = GlobalModel.PolkVehicleImageRootUrl;
                            }
                            else
                            {
                                v.ValidationFailures = new ValidationFailureModel[1];
                                v.ValidationFailures[0] = new ValidationFailureModel();
                                v.ValidationFailures[0].Code = "404";
                                v.ValidationFailures[0].Description = "Data not found";
                                v.IsValid = false;
                            }
                        };
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "There was an error decoding the vin: " + ex.Message;
            }

            if (vin == null || IsVinAValidMaskPattern(vin))
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "426";
                v.ValidationFailures[0].Description = "The VIN supplied is invalid, you cannot retreive a diagnostic report without a valid vin";
                v.IsValid = false;
            }
            if (IsVinAValidMaskPattern(vin) == false)
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "426";
                v.ValidationFailures[0].Description = "The VIN supplied is invalid, you cannot retreive a diagnostic report without a valid vin";
                v.IsValid = false;
            }
            else if (errorMessage.Length > 0 || v == null)
            {
                v.ValidationFailures = new ValidationFailureModel[1];
                v.ValidationFailures[0] = new ValidationFailureModel();
                v.ValidationFailures[0].Code = "420";
                v.ValidationFailures[0].Description = errorMessage;
                v.IsValid = false;
            }

            return v;

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
        public int? GetTSBCountByVehicleCount(string AAIA)
        {
            var count = 0;
            using (innovadev01Entities dbContext = new innovadev01Entities())
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
                                using (innovadev01Entities dbContext = new innovadev01Entities())
                                {
                                    var polkVehicleYMME = dbContext.PolkVehicleYMMEs.Where(x => x.VinPatternMask == apiRequest.vin).FirstOrDefault();
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
                        using (innovadev01Entities dbContext = new innovadev01Entities())
                        {
                            var polkVehicleYMME = dbContext.PolkVehicleYMMEs.Where(x => x.VinPatternMask == apiRequest.vin).FirstOrDefault();
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

                using (innovadev01Entities dbContext = new innovadev01Entities())
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
                                          DTCCodeLaymanTermId = dTCCodeLaymanTerms.DTCCodeLaymanTermId,
                                          Description = dTCCodeLaymanTerms.Description,
                                          SeverityLevel = dTCCodeLaymanTerms.SeverityLevel,
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
                using (innovadev01Entities dbContext = new innovadev01Entities())
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
    }
}