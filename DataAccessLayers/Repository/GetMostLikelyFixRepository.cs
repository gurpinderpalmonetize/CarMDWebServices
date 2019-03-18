using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayers.Repository
{
    public class GetMostLikelyFixRepository
    {
        private innovaEntities _context;

        public GetMostLikelyFixRepository()
        {
        }
        public GetMostLikelyFixRepository(innovaEntities context)
        {
            _context = context;
        }

        public Vehicle GetByDiagnosticReportId(int diagnosticReportId)
        {
            return _context.Vehicles
                               .Join(_context.DiagnosticReports, Vehicle => Vehicle.VehicleId, B => B.VehicleId, (Vehicle, B) => new { Vehicle, B })
                               .Where(e => diagnosticReportId == e.B.DiagnosticReportId).Select(e => e.Vehicle).FirstOrDefault();
        }


        public virtual Task<User> GetUser(Guid userId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                var id = userId.ToString();
                var query = _context.Users.Where(e => e.UserId == id).
                         Join(_context.ExternalSystems, user => user.UserTypeExternalId, external => external.ExternalSystemId, (user, external) => new { user, external }).FirstOrDefault();

                User report = query.user;
                report.ExternalSystem = query.external;
                return Task.FromResult(report);
            }
        }

        public void Save(User user)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                var result = _context.Users.Where(x => x.UserId == user.UserId).Distinct().FirstOrDefault();
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.EmailAddress = user.EmailAddress;
                result.PhoneNumber = user.PhoneNumber;
                result.Region = user.Region;
                _context.Entry(result).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public DiagnosticReport GetDiagnosticReportByReportId(long diagnosticReportId)
        {
            using (innovaEntities _context = new innovaEntities())
            {
                var query = _context.DiagnosticReports
                .Where(e => e.DiagnosticReportId == diagnosticReportId)
                .Join(_context.Vehicles, DiagnosticReport => DiagnosticReport.VehicleId, Vehicle => Vehicle.VehicleId, (DiagnosticReport, Vehicle) => new { DiagnosticReport, Vehicle });
                var result = query.FirstOrDefault();
                DiagnosticReport report = result.DiagnosticReport;
                report.Vehicle = result.Vehicle;
                return report;
            }
        }

        public void SaveDiagnosticReport(DiagnosticReport diagnosticReport)
        {
            if (diagnosticReport.DiagnosticReportId > 0)
            {
                _context.Entry(diagnosticReport).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                diagnosticReport.FirmwareVersion = "";
                diagnosticReport.SoftwareVersion = "";
                diagnosticReport.RawUploadString = "";
                diagnosticReport.PwrMilCode = "";
                _context.Entry(diagnosticReport).State = EntityState.Added;
                _context.SaveChanges();
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        public Vehicle GetVehicleInfoByVinAndUserId(string vin, string userId)
        {
            var patterns = GenerateVinPatternMarks(vin);
            var result = _context.Vehicles
               .Join(_context.PolkVehicleYmmes, A => A.PolkVehicleYMMEId, B => B.PolkVehicleYMMEId, (A, B) => new { A, B })
               .Where(e => (patterns.Contains(e.A.Vin) || patterns.Contains(e.B.VinPatternMask)) && e.A.UserId == userId).FirstOrDefault();

            Vehicle report = result.A;
            report.User = result.A.User;
            report.PolkVehicleYmme = result.B;
            return report;
        }

        public virtual List<string> GenerateVinPatternMarks(string vin)
        {
            var patternMaskList = new List<string>();
            switch (vin.Length)
            {
                case 7:
                    patternMaskList.Add(vin.Substring(0, 3) + "****");
                    break;

                case 8:
                    patternMaskList.Add(vin.Substring(0, 3) + "*****");
                    break;

                case 10:
                    patternMaskList.Add(vin.Substring(0, 4) + "******");
                    patternMaskList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******");
                    break;

                case 11:
                    patternMaskList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 2) + "******");
                    patternMaskList.Add(vin.Substring(0, 8) + "*" + vin.Substring(9, 1) + "*******");
                    patternMaskList.Add(vin.Substring(0, 5) + "******");
                    break;

                case 12:
                    patternMaskList.Add(vin.Substring(0, 6) + "******");
                    patternMaskList.Add(vin.Substring(0, 5) + "*******");
                    break;

                case 13:
                    patternMaskList.Add(vin.Substring(0, 7) + "******");
                    patternMaskList.Add(vin.Substring(0, 6) + "*******");
                    break;

                case 14:
                    patternMaskList.Add(vin.Substring(0, 8) + "******");
                    patternMaskList.Add(vin.Substring(0, 6) + "********");
                    break;

                case 17:
                    var firstEightCharacters = vin.Substring(0, 8);
                    patternMaskList.Add(firstEightCharacters + "*" + vin.Substring(9, 5) + "***");
                    patternMaskList.Add(firstEightCharacters + "*" + vin.Substring(9, 4) + "****");
                    patternMaskList.Add(firstEightCharacters + "*" + vin.Substring(9, 3) + "*****");
                    patternMaskList.Add(firstEightCharacters + "*" + vin.Substring(9, 2) + "******");
                    patternMaskList.Add(firstEightCharacters + "*" + vin.Substring(9, 1) + "*******");
                    break;
            }
            return patternMaskList;
        }

        public virtual Vehicle GetVehicleInfoForVehicleByYearMakeModelAsync(string make, string model, int year, string enginetype)
        {

            var query = _context.Vehicles
                .Join(_context.PolkVehicleYmmes, vehicle => vehicle.PolkVehicleYMMEId, polkVehicleYmme => polkVehicleYmme.PolkVehicleYMMEId, (vehicle, polkVehicleYmme) => new { vehicle, polkVehicleYmme })
                .Join(_context.Users, A => A.vehicle.UserId, user => user.UserId, (A, user) => new { A, user })
                .Where(e => (e.A.polkVehicleYmme.Make == make || e.A.polkVehicleYmme.Make == "")
                && (e.A.polkVehicleYmme.Year == year || e.A.polkVehicleYmme.Year == 0)
                && (e.A.polkVehicleYmme.Model == model || e.A.polkVehicleYmme.Model == "")
                && (e.A.polkVehicleYmme.EngineType == enginetype || e.A.polkVehicleYmme.EngineType == ""));

            var result = query.FirstOrDefault();
            Vehicle report = result.A.vehicle;
            report.User = result.user;
            report.PolkVehicleYmme = result.A.polkVehicleYmme;
            return report;
        }

        public void SaveChanges(Vehicle vehicle)
        {
            var result = _context.Vehicles.Where(e => e.VehicleId == vehicle.VehicleId);
            _context.Entry(result).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Device GetManualDevice(string userId)
        {
            Device device = _context.Devices.Where(x => x.UserId == userId && x.IsManualDevice == true).FirstOrDefault();
            if (device == null)
            {
                device = new Device();
                device.ChipId = Guid.NewGuid().ToString();
                device.IsActive = true;
                device.IsManualDevice = true;
                device.IsPrimaryOwner = true;
                device.UserId = userId;
                _context.Entry(device).State = EntityState.Added;
                _context.SaveChanges();
            }
            return device;
        }

        public ExternalSystem GetPartnerIdbyUderId(string userId)
        {
            return _context.Users
                 .Join(_context.ExternalSystems, user => user.UserTypeExternalId, externalSystem => externalSystem.ExternalSystemId, (user, externalSystem) => new { user, externalSystem })
                 .Where(e => e.user.UserId == userId).Select(x => x.externalSystem).FirstOrDefault();
        }

        public string GetVin(string userId)
        {
            return _context.Users
                 .Join(_context.Vehicles, user => user.UserId, vehicle => vehicle.UserId, (user, vehicle) => new { user, vehicle })
                 .Join(_context.PolkVehicleYmmes, user => user.vehicle.PolkVehicleYMMEId, polkVehicleYmme => polkVehicleYmme.PolkVehicleYMMEId, (user, polkVehicleYmme) => new { user, polkVehicleYmme })
                 .Where(e => e.user.user.UserId == userId).Select(x => x.polkVehicleYmme.VinPatternMask).FirstOrDefault();
        }

        public Device GetDeviceByChipIdAndUserIdAndActive(string toolId, string userId)
        {
            Device device = new Device();
            try
            {
                device = _context.Devices.Where(e => e.UserId == userId && e.ChipId == toolId && e.IsActive == true).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return device;
        }

        public void SaveDevice(Device device)
        {
            if (!string.IsNullOrEmpty(device.DeviceId))
            {
                _context.Entry(device).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                _context.Entry(device).State = EntityState.Added;
                _context.SaveChanges();
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        public List<DiagnosticReportResultErrorCode> GetDiagnosticReportResultErrorCodeCount(string diagnosticReportResultId)
        {
            return _context.DiagnosticReportResultErrorCodes.Where(x => x.DiagnosticReportResultId == diagnosticReportResultId).ToList();
        }
        public List<SymptomDiagnosticReportItem> GetSymptomDiagnosticReportItemByDiagnosticReportId(long diagnosticReportResultId)
        {
            return _context.SymptomDiagnosticReportItems.Where(e => e.DiagnosticReportId == ToGuid(diagnosticReportResultId)).Distinct().ToList();
        }
        public static Guid ToGuid(long value)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(value), guidData, 8);
            return new Guid(guidData);
        }

        public static long ToLong(Guid guid)
        {
            if (BitConverter.ToInt64(guid.ToByteArray(), 8) != 0)
                throw new OverflowException("Value was either too large or too small for an Int64.");
            return BitConverter.ToInt64(guid.ToByteArray(), 0);
        }

        public List<Fix> GetFix_LoadByDiagnosticReportBySymptom(int year, string make, string model, string trimLevel, string transmission,string symptomId, string engineVINCode, string engineType, int market, string generation)
        {
            var FixInfo = _context.Fixes
                    .GroupJoin(_context.FixSymptoms.DefaultIfEmpty(), fix => fix.FixId, fs => fs.FixId, (fix, fs) => new { fix, fs })
                    .GroupJoin(_context.FixEngineTypes.DefaultIfEmpty(), A => A.fix.FixId, fixEngineType => fixEngineType.FixId, (fix, fixEngineType) => new { fix, fixEngineType })
                    .GroupJoin(_context.FixEngineVinCodes.DefaultIfEmpty(), B => B.fix.fix.FixId, fVinCode => fVinCode.FixId, (fix, fVinCode) => new { fix, fVinCode })
                    .GroupJoin(_context.FixYears.DefaultIfEmpty(), C => C.fix.fix.fix.FixId, fixYear => fixYear.FixId, (fix, fixYear) => new { fix, fixYear })
                    .GroupJoin(_context.FixMakes.DefaultIfEmpty(), D => D.fix.fix.fix.fix.FixId, fixMake => fixMake.FixId, (fix, fixMake) => new { fix, fixMake })
                    .GroupJoin(_context.FixModels.DefaultIfEmpty(), E => E.fix.fix.fix.fix.fix.FixId, fixModel => fixModel.FixId, (fix, fixModel) => new { fix, fixModel })
                    .GroupJoin(_context.FixTrimLevels.DefaultIfEmpty(), F => F.fix.fix.fix.fix.fix.fix.FixId, fixTrimLevel => fixTrimLevel.FixId, (fix, fixTrimLevel) => new { fix, fixTrimLevel })
                    .GroupJoin(_context.FixTransmissions.DefaultIfEmpty(), G => G.fix.fix.fix.fix.fix.fix.fix.FixId, fixTransmission => fixTransmission.FixId, (fix, fixTransmission) => new { fix, fixTransmission })
                    .GroupJoin(_context.FixGenerations.DefaultIfEmpty(), H => H.fix.fix.fix.fix.fix.fix.fix.fix.FixId, fixGeneration => fixGeneration.FixId, (fix, fixGeneration) => new { fix, fixGeneration })
                    .Where(I => I.fix.fix.fix.fix.fix.fix.fix.fix.fs.Any(x => x.SymptomId == symptomId)
                        && I.fix.fix.fix.fix.fix.fix.fix.fixEngineType.Any(x => x.EngineType == engineType)
                        && I.fix.fix.fix.fix.fix.fix.fVinCode.Any(x => x.EngineVINCode == engineVINCode)
                        && I.fix.fix.fix.fix.fix.fixYear.Any(x => x.Year == year) && I.fix.fix.fix.fix.fixMake.Any(x => x.Make == make)
                        && I.fix.fix.fix.fixModel.Any(x => x.Model == model) && I.fix.fix.fixTrimLevel.Any(x => x.TrimLevel == trimLevel)
                        && I.fix.fixTransmission.Any(x => x.TransmissionControlType == transmission) && I.fixGeneration.Any(x => x.Generation == generation)
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.MarketsString.Length == 0)
                        && I.fix.fix.fix.fix.fix.fix.fix.fix.fix.IsApproved == true
                        && ((I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasEngineVINCodeDefined == false && I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasEngineTypeDefined == false)
                        || (I.fix.fix.fix.fix.fix.fix.fix.fixEngineType.Any(x => x.FixId != null) || I.fix.fix.fix.fix.fix.fix.fVinCode.Any(x => x.FixId != null))
                        || (engineType.Length == 0 && engineVINCode.Length == 0))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasYearDefined == false || I.fix.fix.fix.fix.fix.fixYear.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasModelDefined == false || model.Length == 0 || I.fix.fix.fix.fixModel.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasTrimLevelDefined == false || trimLevel.Length == 0 || I.fix.fix.fixTrimLevel.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasTransmissionDefined == false || transmission.Length == 0 || I.fix.fixTransmission.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.fix.HasGenerationDefined == false || generation.Length == 0 || I.fixGeneration.Any(x => x.FixId != null))).Distinct();
           return  FixInfo.Select(x => x.fix.fix.fix.fix.fix.fix.fix.fix.fix).OrderBy(x => x.FrequencyCount).ToList();
        }

        public List<Fix> GetLoadByDiagnosticReportUsingVinPowerBySymptom(int year, string make, string model, string trimLevel, string transmission,string symptomId, string engineVINCode, string engineType)
        {
            var FixInfo = _context.Fixes
                    .GroupJoin(_context.FixSymptoms.DefaultIfEmpty(), fix => fix.FixId, FixSymptom => FixSymptom.FixId, (fix, FixSymptom) => new { fix, FixSymptom })
                    .GroupJoin(_context.FixEngineTypeVinPowers.DefaultIfEmpty(), A => A.fix.FixId, FixEngineTypeVinPower => FixEngineTypeVinPower.FixId, (A, FixEngineTypeVinPower) => new { A, FixEngineTypeVinPower })
                    .GroupJoin(_context.FixEngineVINCodeVinPowers.DefaultIfEmpty(), B => B.A.fix.FixId, FixEngineVINCodeVinPower =>  FixEngineVINCodeVinPower.FixId.ToString(), (fix, FixEngineVINCodeVinPower) => new { fix, FixEngineVINCodeVinPower })
                    .GroupJoin(_context.FixYearVINPowers.DefaultIfEmpty(), C => C.fix.A.fix.FixId, FixYearVinPower => FixYearVinPower.FixId, (fix, FixYearVinPower) => new { fix, FixYearVinPower })
                    .GroupJoin(_context.FixMakeVinPowers.DefaultIfEmpty(), D => D.fix.fix.A.fix.FixId, FixMakeVinPower => FixMakeVinPower.FixId, (fix, FixMakeVinPower) => new { fix, FixMakeVinPower })
                    .GroupJoin(_context.FixModelVinPowers.DefaultIfEmpty(), E => E.fix.fix.fix.A.fix.FixId, FixModelVinPower => FixModelVinPower.FixId, (fix, FixModelVinPower) => new { fix, FixModelVinPower })
                    .GroupJoin(_context.FixTrimLevelVinPowers.DefaultIfEmpty(), F => F.fix.fix.fix.fix.A.fix.FixId, FixTrimLevelVinPower => FixTrimLevelVinPower.FixId, (fix, FixTrimLevelVinPower) => new { fix, FixTrimLevelVinPower })
                    .GroupJoin(_context.FixTransmissionVinPowers.DefaultIfEmpty(), G => G.fix.fix.fix.fix.fix.A.fix.FixId, FixTransmissionVinPower => FixTransmissionVinPower.FixId.ToString(), (fix, FixTransmissionVinPower) => new { fix, FixTransmissionVinPower })
                    .Where(H => H.fix.fix.fix.fix.fix.fix.A.FixSymptom.Any(x => x.SymptomId == symptomId)
                        && H.fix.fix.fix.fix.fix.fix.FixEngineTypeVinPower.Any(x => x.EngineType == engineType)
                        && H.fix.fix.fix.fix.fix.FixEngineVINCodeVinPower.Any(x => x.EngineVINCode == engineVINCode)
                        && H.fix.fix.fix.fix.FixYearVinPower.Any(x => x.Year == year && H.fix.fix.fix.FixMakeVinPower.Any(I => I.Make == make))
                        && H.fix.fix.FixModelVinPower.Any(x => x.Model == model && H.fix.FixTrimLevelVinPower.Any(I => I.TrimLevel == trimLevel))
                        && H.FixTransmissionVinPower.Any(x => x.TransmissionControlType == transmission)
                        && H.fix.fix.fix.fix.fix.fix.A.fix.IsApproved == true
                        && H.fix.fix.fix.fix.fix.fix.A.fix.MarketsString.Length == 0
                        && H.fix.fix.fix.fix.fix.fix.A.fix.HasEngineVINCodeDefinedVP == false && H.fix.fix.fix.fix.fix.fix.A.fix.HasEngineTypeDefinedVP == false
                        || H.fix.fix.fix.fix.fix.fix.FixEngineTypeVinPower.Any(x => x.FixId != null) || H.fix.fix.fix.fix.fix.FixEngineVINCodeVinPower.Any(x => x.FixId != null)
                        || engineType.Length == 0 && engineVINCode.Length == 0
                        && (H.fix.fix.fix.fix.fix.fix.A.fix.HasYearDefinedVP == false || H.fix.fix.fix.fix.FixYearVinPower.Any(x => x.FixId != null))
                        && (H.fix.fix.fix.fix.fix.fix.A.fix.HasModelDefinedVP == false || model.Length == 0 || H.fix.fix.FixModelVinPower.Any(x => x.FixId != null))
                        && (H.fix.fix.fix.fix.fix.fix.A.fix.HasTrimLevelDefinedVP == false || trimLevel.Length == 0 || H.fix.FixTrimLevelVinPower.Any(x => x.FixId != null))
                        && (H.fix.fix.fix.fix.fix.fix.A.fix.HasTransmissionDefinedVP == false || transmission.Length == 0 || H.FixTransmissionVinPower.Any(x => x.FixId != null)));
            var result = FixInfo.Select(x => x.fix.fix.fix.fix.fix.fix.A.fix).OrderBy(x => x.FrequencyCount).ToList();
            return result;
        }

        public FixPolkVehicleDiscrepancy GetFixPolkVehicleDiscrepancy_LoadByFixAndVehicle(string fixId, string polkVehicleYMMEId)
        {
            return _context.FixPolkVehicleDiscrepancies.Where(x => x.FixId == fixId && x.PolkVehicleYMMEId == polkVehicleYMMEId).FirstOrDefault();
        }

        public void SaveFixPolkVehicleDiscrepancy(FixPolkVehicleDiscrepancy fixPolkVehicleDiscrepancy)
        {
            var result = _context.FixPolkVehicleDiscrepancies.Where(FixPolkVehicleDiscrepancy => FixPolkVehicleDiscrepancy.FixPolkVehicleDiscrepancyId == fixPolkVehicleDiscrepancy.FixPolkVehicleDiscrepancyId).Distinct().FirstOrDefault();
            _context.Entry(result).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual List<User> GetOBDFixMasterTechs(string userId, string make)
        {
            if (!string.IsNullOrWhiteSpace(make))
                return _context.Users.Where(x => x.MasterTechMakesString.ToLower().Contains(make.ToLower()) && x.UserId == userId).Distinct().ToList();
            else
                return _context.Users.Where(x => x.UserId == userId ).Distinct().ToList();
        }

        public void SaveUser(User user)
        {
            var result = _context.Users.Where(User => User.UserId == user.UserId);
            _context.Entry(result).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<DiagnosticReportResultFix> GetDiagnosticReportResultFix(string diagnosticReportResultId)
        {
            return _context.DiagnosticReportResultFixes.Where(x => x.DiagnosticReportResultId == diagnosticReportResultId).ToList();
        }

        public List<FixPart> GetFixPartByFixId(string fixId)
        {
            return _context.FixParts.Where(e =>e.PartId == fixId).Distinct().ToList();
        }

        public FixName GetByFixNameId(string fixNameId)
        {
            return _context.FixNames.Where(x => x.FixNameId == fixNameId).FirstOrDefault();
        }

        public StateLaborRate GetStateLaborByStateCode(string stateCode)
        {
            return _context.StateLaborRates.Where(e =>(e.StateCode == stateCode)).Distinct().FirstOrDefault();
        }

        public decimal GetCurrencyExchangeRate(string CurrenctISOCode)
        {
            return _context.CurrencyExchangeRates.Where(e => (e.CurrencyISOCode == CurrenctISOCode)).Distinct().Select(x => x.ExchangeRatePerUSD).FirstOrDefault();
        }

        public Part GetPartByPartId(string PartId)
        {
            return _context.Parts.Where(x => x.PartId == PartId).FirstOrDefault();
        }

        public void SaveDiagnosticReportResult(long diagnosticReportId)
        {
            DiagnosticReportResult objDiagnosticReportResult = new DiagnosticReportResult();
            objDiagnosticReportResult.DiagnosticReportId = diagnosticReportId;
            objDiagnosticReportResult.CreatedDateTimeUTC = DateTime.UtcNow;
            _context.Entry(objDiagnosticReportResult).State = EntityState.Added;
            _context.SaveChanges();
        }

        public List<Fix> GetFix_LoadByDiagnosticReportUsingVinPower(int year, string make, string model, string trimLevel, string transmission, string primaryCode, string engineVINCode, string engineType, int market)
        {
            var FixInfo = _context.Fixes
                    .GroupJoin(_context.FixDTCs.DefaultIfEmpty(), fix => fix.FixId, fixDTC => fixDTC.FixId, (fix, fixDTC) => new { fix, fixDTC })
                    .GroupJoin(_context.FixEngineTypeVinPowers.DefaultIfEmpty(), A => A.fix.FixId, fixEngineType => fixEngineType.FixId, (fix, fixEngineType) => new { fix, fixEngineType })
                    .GroupJoin(_context.FixEngineVINCodeVinPowers.DefaultIfEmpty(), B => B.fix.fix.FixId, fVinCode => fVinCode.FixId.ToString(), (fix, fVinCode) => new { fix, fVinCode })
                    .GroupJoin(_context.FixYearVINPowers.DefaultIfEmpty(), C => C.fix.fix.fix.FixId, fixYear => fixYear.FixId, (fix, fixYear) => new { fix, fixYear })
                    .GroupJoin(_context.FixMakeVinPowers.DefaultIfEmpty(), D => D.fix.fix.fix.fix.FixId, fixMake => fixMake.FixId, (fix, fixMake) => new { fix, fixMake })
                    .GroupJoin(_context.FixModelVinPowers.DefaultIfEmpty(), E => E.fix.fix.fix.fix.fix.FixId, fixModel => fixModel.FixId, (fix, fixModel) => new { fix, fixModel })
                    .GroupJoin(_context.FixTrimLevelVinPowers.DefaultIfEmpty(), F => F.fix.fix.fix.fix.fix.fix.FixId, fixTrimLevel => fixTrimLevel.FixId.ToString(), (fix, fixTrimLevel) => new { fix, fixTrimLevel })
                    .GroupJoin(_context.FixTransmissionVinPowers.DefaultIfEmpty(), G => G.fix.fix.fix.fix.fix.fix.fix.FixId, fixTransmission => fixTransmission.FixId.ToString(), (fix, fixTransmission) => new { fix, fixTransmission })
                    .Where(I => I.fix.fix.fix.fix.fix.fix.fix.fixDTC.Any(x => x.PrimaryDTC == primaryCode)
                        && I.fix.fix.fix.fix.fix.fix.fixEngineType.Any(x => x.EngineType == engineType)
                        && I.fix.fix.fix.fix.fix.fVinCode.Any(x => x.EngineVINCode == engineVINCode)
                        && I.fix.fix.fix.fix.fixYear.Any(x => x.Year == year) && I.fix.fix.fix.fixMake.Any(x => x.Make == make)
                        && I.fix.fix.fixModel.Any(x => x.Model == model) && I.fix.fixTrimLevel.Any(x => x.TrimLevel == trimLevel)
                        && I.fixTransmission.Any(x => x.TransmissionControlType == transmission)
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.MarketsString.Length == 0)
                        && I.fix.fix.fix.fix.fix.fix.fix.fix.IsApproved == true
                        && ((I.fix.fix.fix.fix.fix.fix.fix.fix.HasEngineVINCodeDefinedVP == false && I.fix.fix.fix.fix.fix.fix.fix.fix.HasEngineTypeDefinedVP == false)
                        || (I.fix.fix.fix.fix.fix.fix.fixEngineType.Any(x => x.FixId != null) || I.fix.fix.fix.fix.fix.fVinCode.Any(x => x.FixId != null))
                        || (engineType.Length == 0 && engineVINCode.Length == 0))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.HasYearDefinedVP == false || I.fix.fix.fix.fix.fixYear.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.HasModelDefinedVP == false || model.Length == 0 || I.fix.fix.fixModel.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.HasTrimLevelDefinedVP == false || trimLevel.Length == 0 || I.fix.fixTrimLevel.Any(x => x.FixId != null))
                        && (I.fix.fix.fix.fix.fix.fix.fix.fix.HasTransmissionDefinedVP == false || transmission.Length == 0 || I.fixTransmission.Any(x => x.FixId != null))).Distinct();
       return FixInfo.Select(x => x.fix.fix.fix.fix.fix.fix.fix.fix).OrderBy(x => x.FrequencyCount).ToList();

        }

        public List<DTCCode> GetDTCCode_LoadByDiagnosticReportAndErrorCodes(int year, string make, string model, string transmission, string engineType, string engineVINCode, string trimLevel, List<string> errorCodes)
        {
            var DTCCodeInfo = _context.DTCCodes
                    .GroupJoin(_context.DTCMakes.DefaultIfEmpty(), dtcCode => dtcCode.DTCCodeId, dtcMake => dtcMake.DTCCodeId, (dtcCode, dtcMake) => new { dtcCode, dtcMake })
                    .GroupJoin(_context.DTCEngineTypes.DefaultIfEmpty(), A => A.dtcCode.DTCCodeId, dTCEngineType => dTCEngineType.DTCCodeId, (dtcCode, dTCEngineType) => new { dtcCode, dTCEngineType })
                    .GroupJoin(_context.DTCEngineVINCodes.DefaultIfEmpty(), B => B.dtcCode.dtcCode.DTCCodeId, dtcEngineVINCode => dtcEngineVINCode.DTCCodeId.ToString(), (dtcCode, dtcEngineVINCode) => new { dtcCode, dtcEngineVINCode })
                    .GroupJoin(_context.DTCYears.DefaultIfEmpty(), C => C.dtcCode.dtcCode.dtcCode.DTCCodeId, dtcYear => dtcYear.DTCCodeId, (dtcCode, dtcYear) => new { dtcCode, dtcYear })
                    .GroupJoin(_context.DTCModels.DefaultIfEmpty(), D => D.dtcCode.dtcCode.dtcCode.dtcCode.DTCCodeId, dtcModel => dtcModel.DTCCodeId, (dtcCode, dtcModel) => new { dtcCode, dtcModel })
                    .GroupJoin(_context.DTCTransmissions.DefaultIfEmpty(), E => E.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.DTCCodeId, dtcTransmission => dtcTransmission.DTCCodeId, (dtcCode, dtcTransmission) => new { dtcCode, dtcTransmission })
                    .GroupJoin(_context.DTCTrimLevels.DefaultIfEmpty(), F => F.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.DTCCodeId, dtcTrimLevel => dtcTrimLevel.DTCCodeId, (dtcCode, dtcTrimLevel) => new { dtcCode, dtcTrimLevel })
                     .Where(G => G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcMake.Any(x => x.Make == make)
                        && G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dTCEngineType.Any(x => x.EngineType == engineType)
                        && G.dtcCode.dtcCode.dtcCode.dtcCode.dtcEngineVINCode.Any(x => x.EngineVINCode == engineVINCode)
                        && G.dtcCode.dtcCode.dtcCode.dtcYear.Any(x => x.Year == year)
                        && G.dtcCode.dtcCode.dtcModel.Any(x => x.Model == model)
                        && G.dtcCode.dtcTransmission.Any(x => x.TransmissionControlType == transmission)
                        && G.dtcTrimLevel.Any(x => x.TrimLevel == trimLevel)
                        && errorCodes.Contains(G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.ErrorCode)
                        && G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.IsApproved == true
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasEngineTypeDefined == false || G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dTCEngineType.Any(x => x.DTCCodeId != null))
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasEngineVINCodeDefined == false || G.dtcCode.dtcCode.dtcCode.dtcCode.dtcEngineVINCode.Any(x => x.DTCCodeId != null))
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasYearDefined == false || G.dtcCode.dtcCode.dtcCode.dtcYear.Any(x => x.DTCCodeId != null))
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasModelDefined == false || G.dtcCode.dtcCode.dtcModel.Any(x => x.DTCCodeId != null))
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasTransmissionDefined == false || G.dtcCode.dtcTransmission.Any(x => x.DTCCodeId != null))
                        && (G.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.HasTrimLevelDefined == false || G.dtcTrimLevel.Any(x => x.DTCCodeId != null))).Distinct();
          return DTCCodeInfo.Select(x => x.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode.dtcCode).OrderBy(x => x.FrequencyCount).ToList();

        }

        public List<DTCMasterCodeList> GetDTCMasterCodeList_LoadByDiagnosticReportAndErrorCodes(string make, List<string> errorCodes)
        {
            var result = _context.DTCMasterCodeLists
                           .Join(_context.DTCMasterCodeMakes.DefaultIfEmpty(), dtcMasterCodeList => dtcMasterCodeList.DTCMasterCodeId, dtcMasterCodeMake => dtcMasterCodeMake.DTCMasterCodeId.ToString(), (dtcMasterCodeList, dtcMasterCodeMake) => new { dtcMasterCodeList, dtcMasterCodeMake })
                           .Where(x => x.dtcMasterCodeMake.Make == make).Select(x => x.dtcMasterCodeList.DTCMasterCodeId).Distinct().ToList();

            var DTCMasterCodeListData = _context.DTCMasterCodeLists.Where(x => result.Contains(x.DTCMasterCodeId) && errorCodes.Contains(x.ErrorCode))
                            .Select(x => x.DTCMasterCodeId).Distinct().ToList();

            var result2 = _context.DTCMasterCodeLists
                            .Where(x => DTCMasterCodeListData.Contains(x.DTCMasterCodeId) && x.ManufacturerName == "Generic"
                             && !errorCodes.Contains(x.ErrorCode)).Select(x => x.DTCMasterCodeId).Distinct().ToList();

           return _context.DTCMasterCodeLists.Where(x => result2.Contains(x.DTCMasterCodeId)).Distinct().ToList();

        }

        public List<VehicleType> GetVehicleType_LoadByVinData(int year, string make, string model, string engineVINCode, string transmissionType, string engineType, string bodyCode)
        {
            return _context.VehicleTypes
                        .Where(x => x.VehicleTypeStatus == 0
                        && x.Year == year && x.Make == make && x.Model != model
                        && (x.TransmissionControlType == transmissionType || x.TransmissionControlType.Length == 0)
                        && ((x.EngineType == engineType || x.EngineTypeVINLookup.IndexOf(engineType) > 0 || engineType.IndexOf(x.EngineTypeVINLookup) > 0 || x.EngineType.IndexOf(engineType) > 0 || engineType.IndexOf(x.EngineType) > 0)
                        && x.EngineVINCode == engineVINCode) && x.BodyCode == bodyCode)
                        .Concat(_context.VehicleTypes
                        .Where(x => x.VehicleTypeStatus == 0 && x.Year == year && x.Make == make && x.Model == model
                        && (x.TransmissionControlType == transmissionType || x.TransmissionControlType.Length == 0)
                        && ((x.EngineType == engineType || x.EngineTypeVINLookup.IndexOf(engineType) > 0 || engineType.IndexOf(x.EngineTypeVINLookup) > 0 || x.EngineType.IndexOf(engineType) > 0 || engineType.IndexOf(x.EngineType) > 0)
                        && x.EngineVINCode == engineVINCode) && x.BodyCode == bodyCode)).Distinct().ToList();
        }

        public List<VehicleTypeCodeAssignment> GetVehicleTypeCodeAssignment(List<VehicleType> vehicleTypes, List<string> ErrorCode)
        {
            return _context.VehicleTypeCodeAssignments.Where(x => vehicleTypes.Any(y => y.VehicleTypeId == x.VehicleTypeId) && ErrorCode.Contains(x.ErrorCode)).ToList();
        }

        public List<DiagnosticReportResultErrorCode> GetOldDiagnosticErrorCode(long diagnosticCodeId)
        {
           return _context.DiagnosticReportResults
                .Join(_context.DiagnosticReportResultErrorCodes, diagnosticReportResult => diagnosticReportResult.DiagnosticReportResultId, diagnosticReportResultErrorCode => diagnosticReportResultErrorCode.DiagnosticReportResultId, (diagnosticReportResult, diagnosticReportResultErrorCode) => new { diagnosticReportResult, diagnosticReportResultErrorCode })
                .Select(x => x.diagnosticReportResultErrorCode).ToList();
        }

        public DTCCodeLaymanTerm GetDTCCodeLaymanTermLoadByErrorCodeAndMake(string ErrorCode, string Make)
        {
           return  _context.DTCCodeLaymanTerms
                    .Join(_context.DTCCodeLaymanTermSeverityDefinitions, dTCCodeLaymanTerm => dTCCodeLaymanTerm.SeverityLevel, A => A.SeverityLevel, (dTCCodeLaymanTerm, A) => new { dTCCodeLaymanTerm, A })
                    .Where(x => x.dTCCodeLaymanTerm.ErrorCode == ErrorCode && x.dTCCodeLaymanTerm.Make == Make || x.dTCCodeLaymanTerm.Make == null).Select(e => e.dTCCodeLaymanTerm).Distinct().FirstOrDefault();
        }

        public PolkVehicleYmme GetByPolkVehicleYMMEId(string polkVehicleYMMEId)
        {
            return _context.PolkVehicleYmmes.Where(x => x.PolkVehicleYMMEId == polkVehicleYMMEId).FirstOrDefault();
        }


        public List<DiagnosticReportResultErrorCode> GetDiagnosticReportResultErrorCode(string diagnosticReportResultId)
        {
         return _context.DiagnosticReports
                         .Join(_context.DiagnosticReportResults, diagnosticReport => diagnosticReport.DiagnosticReportId, B => B.DiagnosticReportId, (diagnosticReport, B) => new { diagnosticReport, B })
                         .Join(_context.DiagnosticReportResultErrorCodes, A => A.diagnosticReport.DiagnosticReportResultId, diagnosticReportResultErrorCode => diagnosticReportResultErrorCode.DiagnosticReportResultId, (A, diagnosticReportResultErrorCode) => new { A, diagnosticReportResultErrorCode }).Where(x => x.A.diagnosticReport.DiagnosticReportResultId == diagnosticReportResultId)
                         .Select(e => e.diagnosticReportResultErrorCode).Distinct().ToList();
  
        }
        public List<Symptom> GetSymptomRecords(string diagnosticReportResultId)
        {
          return _context.DiagnosticReports
                              .Join(_context.Symptoms, diagnosticReport => diagnosticReport.SymptomId, Symptom => Symptom.SymptomId, (diagnosticReport, Symptom) =>
                               new { diagnosticReport, Symptom }).Where(x => x.diagnosticReport.DiagnosticReportResultId == diagnosticReportResultId)
                              .Select(e => e.Symptom).Distinct().ToList();
        }

        public List<DiagnosticReportResultFix> GetDiagnosticReportResultFixes(string diagnosticReportResultId)
        {
          return _context.DiagnosticReportResultFixes.Where(x => x.DiagnosticReportResultId == diagnosticReportResultId)
                         .OrderByDescending(x => x.DiagnosticReportErrorCodeSystemType)
                         .Distinct()
                         .ToList();

        }

        public virtual decimal GetCurrenctISOCode(string CurrenctISOCode)
        {
         return _context.CurrencyExchangeRates.Where(e => (e.CurrencyISOCode == CurrenctISOCode)).Distinct().Select(x => x.ExchangeRatePerUSD).FirstOrDefault();
        }


        public virtual List<DiagnosticReportFixFeedback> LoadByFixAndDtc(string fixId, string primaryErrorCode)
        {
            return (from a in _context.DiagnosticReportFixFeedbacks
                    join b in _context.ObdFixes on a.ObdFixId equals b.ObdFixId
                    where a.FixId == fixId && a.FixId == fixId && a.PrimaryErrorCode == primaryErrorCode
                    select a).ToList();

        }

        public virtual List<Article> GetRelatedArticles(string FixNameId)
        {
        return _context.Articles
                    .Join(_context.FixNameArticleAssignments, Article => Article.ArticleId, fixName => fixName.ArticleId, (Article, fixName) =>
                     new { Article, fixName }).Where(x => x.fixName.FixNameId == FixNameId)
                    .Select(e => e.Article).Distinct().ToList();

        }

        public List<string> GetByDiagnosticReportResultFixId(string diagnosticReportResultFixId)
        {
            return new List<string>(); //_context.DiagnosticReportResultFixParts.Where(x => x.DiagnosticReportResultFixId == diagnosticReportResultFixId).Distinct().ToList();
        }

        public List<Recall> Search(int year, string make, string model)
        {
            int? years = Convert.ToInt32(year);
            return _context.Recalls
                .Join(_context.Recalls, Recall => Recall.RecordNumber, B => B.RecordNumber, (Recall, B) => new { Recall, B })
                          .Where(e => (((year == 0 || e.Recall.Year == years)))                             //filter by years
                          && (((make == null || make.Length == 0) || make.Contains(e.Recall.Make)))        //filter by makes 
                          && (((model == null || model.Length == 0) || model.Contains(e.Recall.Model))))   //filter by models 
                .Select(e => e.Recall).Distinct().ToList();
        }

        public List<VehicleWarrantyInfo> GetCurrentlyValidWarranty(Vehicle vehicle, int averageMilesDrivenPerDay)
        {
            var currentmillage = GetEstimatedMileage(vehicle, new DateTime(), averageMilesDrivenPerDay);
            var VehicleAgeInYears = DateTime.Now.Year - vehicle.Year;


            var warrantyInfo = _context.VehicleWarranties
                   .Join(_context.VehicleWarrantyDetails, VehicleWarranty => VehicleWarranty.VehicleWarrantyId, vehicleWarrantyDetail => vehicleWarrantyDetail.VehicleWarrantyId, (VehicleWarranty, vehicleWarrantyDetail) => new { VehicleWarranty, vehicleWarrantyDetail })
                    .Join(_context.VehicleWarrantyMakes, A => A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Join(_context.VehicleWarrantyModels, A => A.A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Join(_context.VehicleWarrantyEngineTypes, A => A.A.A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Join(_context.VehicleWarrantyEngineVINCodes, A => A.A.A.A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Join(_context.VehicleWarrantyTrimLevels, A => A.A.A.A.A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Join(_context.VehicleWarrantyTransmissions, A => A.A.A.A.A.A.VehicleWarranty.VehicleWarrantyId.DefaultIfEmpty(), B => B.VehicleWarrantyId, (A, B) => new { A, B })
                    .Where(e => (vehicle.Year == 0 || e.A.A.A.A.A.A.VehicleWarranty.MinYear >= vehicle.Year || e.A.A.A.A.A.A.VehicleWarranty.MaxYear <= vehicle.Year)
                    && (vehicle.Make == null || vehicle.Make.Length == 0 || vehicle.Make.Contains(e.A.A.A.A.A.A.VehicleWarranty.MakesString))
                    && (vehicle.Model == null || vehicle.Model.Length == 0 || (vehicle.Model.Contains(e.A.A.A.A.A.A.VehicleWarranty.ModelsString))
                    && (vehicle.TrimLevel == null || vehicle.TrimLevel.Length == 0 || (vehicle.TrimLevel.Contains(e.A.A.A.A.A.A.VehicleWarranty.TrimLevelsString))
                    && (vehicle.TransmissionControlType == null || vehicle.TransmissionControlType.Length == 0 || (vehicle.TransmissionControlType.Contains(e.A.A.A.A.A.A.VehicleWarranty.TransmissionsString))
                    && (currentmillage == 0 || e.A.A.A.A.A.A.vehicleWarrantyDetail.MaxMileage >= currentmillage)
                    && (VehicleAgeInYears == 0 || e.A.A.A.A.A.A.vehicleWarrantyDetail.MaxYears >= VehicleAgeInYears))))).Distinct();

            var result = warrantyInfo.ToList();
            var vehicleWarranty = result.Select(x => new VehicleWarrantyInfo
            {
                VehicleWarranty = x.A.A.A.A.A.A.VehicleWarranty,
                VehicleWarrantyDetail = x.A.A.A.A.A.A.vehicleWarrantyDetail
            }).ToList();

            return vehicleWarranty;
        }


        public int GetEstimatedMileage(Vehicle vehicleInfo, DateTime date, int milesDrivenPerDay)
        {
            if (vehicleInfo.Mileage == 0)
                vehicleInfo.Mileage = GetLastVehicleMileage(vehicleInfo);
            TimeSpan ts = date.Subtract(Convert.ToDateTime(vehicleInfo.MileageLastRecordedDateTimeUTC));
            int daysSinceLastMileage = (int)ts.TotalDays;
            return Convert.ToInt32(vehicleInfo.Mileage) + (daysSinceLastMileage * milesDrivenPerDay);
        }

        public int? GetLastVehicleMileage(Vehicle info)
        {
            return _context.DiagnosticReports.Where(e => info.VehicleId == e.VehicleId && info.UserId == e.UserId).Select(x => x.VehicleMileage).FirstOrDefault();
        }

        public virtual List<TSB> GetTSBCountByVehicleByCategory(int legacyVehicleId)
        {
             return _context.TSBs
                .Join(_context.TSBToVehicles, tsb => tsb.TSBID, tsbtovehicle => tsbtovehicle.TSBID,
                    (tsb, tsbtovehicle) => new { tsb, tsbtovehicle })
                .Join(_context.TSBAAIAToLegacyVehicleIDs, tsbtovehicle => tsbtovehicle.tsbtovehicle.VehicleId,
                    tsblegacy => tsblegacy.LegacyVehicleID, (tsbtovehicle, tsblegacy) => new { tsbtovehicle, tsblegacy })
                .Where(e => e.tsbtovehicle.tsbtovehicle.VehicleId == legacyVehicleId)
                .Select(e => e.tsbtovehicle.tsb)
                .Distinct().GroupBy(x => x.TSBID).Select(x => x.FirstOrDefault()).ToList();
        }

        public int GetTSBCountAll(int AAIA)
        {
            return GetTSBCountByVehicleByCategory(AAIA).Count;
        }

        public List<TSBInfo> GetTSBCategory(int AAIA)
        {
            var category = GetTSBCountByVehicleByCategory(AAIA);
           return category.Select(item => new TSBInfo()
            {
                TsbId = item.TSBID,
                TsbText = item.TSBText,
                PDFFileUrl = item.FileNamePDF,
                Description = item.Description,
                IssueDateString = Convert.ToString( item.IssueDate),
                ManufacturerNumber = item.ManufacturerNumber,
            }).ToList();
        }
    }
}
