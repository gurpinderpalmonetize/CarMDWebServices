using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;
using System;
using System.Linq;

namespace DataAccessLayers.WebService
{
    public  class DiagnosticReportWebService
    {

        public static bool IsDuplicate(Guid? Id, ApiRequestModel apiRequest)
        {
            bool flag = false;
            var id = Convert.ToString(Id);
            using (innovaEntities dbContext = new innovaEntities())
            {
                var vinDecoder = (from diagnosticReports in dbContext.DiagnosticReports join user in dbContext.Users on diagnosticReports.UserId equals user.UserId
                                  join externalSystems in dbContext.ExternalSystems on user.UserTypeExternalId equals externalSystems.ExternalSystemId
                                  join vehicles in dbContext.Vehicles on diagnosticReports.VehicleId equals vehicles.VehicleId
                                  join diagnosticId in dbContext.DiagnosticReportExternalSystemReportIds on diagnosticReports.DiagnosticReportId equals diagnosticId.DiagnosticReportId
                                  where externalSystems.KeyGuid == id
                                  && vehicles.Vin == apiRequest.vin
                                 && diagnosticReports.RawUploadString == apiRequest.rawToolPayload
                                && diagnosticId.ExternalSystemReportId == apiRequest.reportID
                                  select diagnosticReports.DiagnosticReportId).Distinct().FirstOrDefault();
                if (vinDecoder != 0)
                {
                    return true;
                }
            };
            return flag;
        }


        public static DiagReportInfo GetDiagnosticReport(ApiRequestModel apiRequest)
        {
            DiagReportInfo v = new DiagReportInfo();
            using (innovaEntities dbContext = new innovaEntities())
            {
                var report = (from vehicles in dbContext.Vehicles
                              join diagnosticReports in dbContext.DiagnosticReports
                              on vehicles.VehicleId equals diagnosticReports.VehicleId
                              where vehicles.Vin == apiRequest.vin
                              select new DiagReportInfo
                              {
                                  DiagnosticReportId = diagnosticReports.DiagnosticReportId,
                                  ScheduledMaintenanceNextMileage = true,
                                  HasScheduledMaintenance = true,
                                  HasUnScheduledMaintenance = true,
                                  HasVehicleWarrantyDetails = true,
                                  UnScheduledMaintenanceNextMileage = true,
                                  IsValid = true
                              }).Distinct().FirstOrDefault();
                if(report.DiagnosticReportId == 0)
                {
                    report.DiagnosticReportId = 0;
                    report.ScheduledMaintenanceNextMileage = false;
                    report.HasScheduledMaintenance = false;
                    report.HasUnScheduledMaintenance = false;
                    report.HasVehicleWarrantyDetails = false;
                    report.UnScheduledMaintenanceNextMileage = false;
                    return report;
                }
                if (report.DiagnosticReportId != 0)
                       return report;
            }

            return v;
        }
        public static DiagReportInfo GetMileage(ApiRequestModel apiRequest)
        {
            DiagReportInfo v = new DiagReportInfo();
            using (innovaEntities dbContext = new innovaEntities())
            {
                var report = (from vehicles in dbContext.Vehicles
                              join diagnosticReports in dbContext.DiagnosticReports
                              on vehicles.VehicleId equals diagnosticReports.VehicleId
                              where vehicles.Vin == apiRequest.vin && vehicles.Mileage == apiRequest.vehicleMileage
                            //  && vehicles.MileageLastRecordedDateTimeUTC == apiRequest.createdDateTime
                              select new DiagReportInfo
                              {
                                  DiagnosticReportId = diagnosticReports.DiagnosticReportId,
                                  ScheduledMaintenanceNextMileage = true,
                                  HasScheduledMaintenance = true,
                                  HasUnScheduledMaintenance = true,
                                  HasVehicleWarrantyDetails = true,
                                  UnScheduledMaintenanceNextMileage = true,
                                  IsValid = true
                              }).Distinct().FirstOrDefault();
                if (report.DiagnosticReportId == 0)
                {
                    report.DiagnosticReportId = 0;
                    report.ScheduledMaintenanceNextMileage = false;
                    report.HasScheduledMaintenance = false;
                    report.HasUnScheduledMaintenance = false;
                    report.HasVehicleWarrantyDetails = false;
                    report.UnScheduledMaintenanceNextMileage = false;
                    return report;
                }
                if (report.DiagnosticReportId != null)
                {
                    return report;
                }
            }

            return v;
        }
    }
}
