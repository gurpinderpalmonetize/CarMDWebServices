using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayers.WebService;
using DataAccessLayers.Model;
using DataAccessLayers.DataObjects;
using System.Xml;

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
        public VehicleInfoModel GetVehicleInfo (string key,string vin)
        {
            var v = new VehicleInfoModel();

            if (ValidateKeyAndLogTransaction(key) == null )
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

            if (errorMessage.Length > 0 || v == null)
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
                recalls = RecallWebService.Search(year, make, model, trimLevel);
            }
            else
            {
                 recalls = RecallWebService.GetByYearMakeModelDateRange(year, make, model, startDateString, endDateString);
            }
            return recalls.Count;
        }
    }
}
