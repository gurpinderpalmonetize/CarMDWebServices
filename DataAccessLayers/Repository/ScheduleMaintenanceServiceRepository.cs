using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;
using DataAccessLayers.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayers.Repository
{
    public  class ScheduleMaintenanceServiceRepository
    {

        private innovaEntities _context;
        private GetMostLikelyFixRepository _getMostLikelyFixRepository;
        private DiagnosticReportService _diagnosticReportService;
        

        public ScheduleMaintenanceServiceRepository(innovaEntities context, GetMostLikelyFixRepository getMostLikelyFixRepository, DiagnosticReportService diagnosticReportService)
        {
            _context = context;
            _getMostLikelyFixRepository = getMostLikelyFixRepository;
            _diagnosticReportService = diagnosticReportService;
        }

        public virtual List<ScheduleMaintenanceServiceInfo> GetScheduledMaintenanceNextService(int[] diagnosticReportId)
        {
        
            Vehicle vehicle = _getMostLikelyFixRepository.GetByDiagnosticReportId(diagnosticReportId[0]);
            var planInfo = GetScheduledMaintenanceNextServiceAsyc(vehicle);
            if (planInfo != null)
            {
                var scheduleMaintenanceServiceInfos = GetNextServices(vehicle, planInfo, true, DateTime.UtcNow, 0, false);
                return scheduleMaintenanceServiceInfos;
            }
            return new List<ScheduleMaintenanceServiceInfo>();
        }

        public virtual ScheduleMaintenancePlan GetScheduledMaintenanceNextServiceAsyc(Vehicle vehicle)
        {
            return _context.ScheduleMaintenancePlans
                 .GroupJoin(_context.ScheduleMaintenancePlanEngineTypes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanEngineType => ScheduleMaintenancePlanEngineType.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineType) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineType })
                 .GroupJoin(_context.ScheduleMaintenancePlanEngineVINCodes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanEngineVINCode => ScheduleMaintenancePlanEngineVINCode.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineVINCode) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanEngineVINCode })
                 .GroupJoin(_context.ScheduleMaintenancePlanMakes.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanMake => ScheduleMaintenancePlanMake.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanMake) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanMake })
                 .GroupJoin(_context.ScheduleMaintenancePlanModels.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanModel => ScheduleMaintenancePlanModel.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanModel) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanModel })
                 .GroupJoin(_context.ScheduleMaintenancePlanYears.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanYear => ScheduleMaintenancePlanYear.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanYear) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanYear })
                 .GroupJoin(_context.ScheduleMaintenancePlanTrimLevels.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanTrimLevel => ScheduleMaintenancePlanTrimLevel.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanTrimLevel) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanTrimLevel })
                 .GroupJoin(_context.ScheduleMaintenancePlanTransmissions.DefaultIfEmpty(), ScheduleMaintenancePlan => ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanId, ScheduleMaintenancePlanTransmission => ScheduleMaintenancePlanTransmission.ScheduleMaintenancePlanId, (ScheduleMaintenancePlan, ScheduleMaintenancePlanTransmission) => new { ScheduleMaintenancePlan, ScheduleMaintenancePlanTransmission })
                 .Where(e =>
                    (vehicle.Year == 0 || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanYear.Any(x => x.Year == vehicle.Year))                                                                                  //filter by YearsString
                    && (vehicle.Make == null || vehicle.Make.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanMake.Any(x => x.Make == vehicle.Make)                                           //filter by MakesString 
                    && (vehicle.Model == null || vehicle.Model.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanModel.Any(x => x.Model == vehicle.Model)                                       //filter by ModelsString 
                    && (vehicle.EngineType == null || vehicle.EngineType.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanEngineType.Any(x => x.EngineType == vehicle.EngineType)                   //filter by EngineTypesString 
                    && (vehicle.TrimLevel == null || vehicle.TrimLevel.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlanTrimLevel.Any(x => x.TrimLevel == vehicle.TrimLevel)                   //filter by TrimLevelsString 
                      && (vehicle.EngineVINCode == null || vehicle.EngineVINCode.Length == 0) || e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlanEngineVINCode.Any(x => x.EngineVINCode == vehicle.EngineVINCode)
                    && (vehicle.TransmissionControlType == null || vehicle.TransmissionControlType.Length == 0) || e.ScheduleMaintenancePlanTransmission.Any(x => x.Transmission == vehicle.TransmissionControlType)
                    && e.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.ScheduleMaintenancePlan.Type == 0)           //filter by TransmissionsString 
                   .Distinct()
                 .FirstOrDefault()?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan?.ScheduleMaintenancePlan;

        }

        public virtual List<ScheduleMaintenanceServiceInfo> GetNextServices(Vehicle vehicleInfo, ScheduleMaintenancePlan planInfo, bool lookupFixesPartsAndLabor, DateTime today, int milesDrivenPerDay, bool useSingleDayMileageWindow)
        {
            int mileage = _getMostLikelyFixRepository.GetEstimatedMileage(vehicleInfo, today, milesDrivenPerDay);
            List<ScheduleMaintenancePlanDetail> planDetails = GetByPlanId(planInfo.ScheduleMaintenancePlanId);
            var PlanDetails = new List<ScheduleMaintenancePlanDetail>();
            var nextServicesPlanDetails = new List<ScheduleMaintenancePlanDetail>();

            int nextMileageInterval = 0;
            foreach (ScheduleMaintenancePlanDetail plan in planDetails)
            {
                int mileageMax = 9999999;
                nextMileageInterval = _diagnosticReportService.GetNextServiceMileageInterval(mileage, plan.Mileage, plan.MileageRepeat);
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
                planNextMileage = _diagnosticReportService.GetNextServiceMileageInterval(mileage, plan.Mileage, plan.MileageRepeat);
                if (nextMileageInterval == 0 || planNextMileage < nextMileageInterval)
                {
                    nextMileageInterval = planNextMileage;
                }
            }

            foreach (ScheduleMaintenancePlanDetail plan in planDetails)
            {
                if (_diagnosticReportService.GetNextServiceMileageInterval(mileage, plan.Mileage, plan.MileageRepeat) == nextMileageInterval)
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

        public List<ScheduleMaintenancePlanDetail> GetByPlanId(string planId)
        {
            return (from s in _context.ScheduleMaintenancePlans
                    join b in _context.ScheduleMaintenancePlanDetails
                    on s.ScheduleMaintenancePlanId equals b.ScheduleMaintenancePlanId
                    where s.ScheduleMaintenancePlanId == planId
                    select b).ToList();
        }

        public ScheduleMaintenanceServiceInfo GetWebServiceObject(ScheduleMaintenancePlanDetail sdkObject, int mileage)
        {
            ScheduleMaintenanceServiceInfo wsObject = new ScheduleMaintenanceServiceInfo();
            var fix = GetFixByServiceId(sdkObject.ScheduleMaintenanceServiceId);
            var fixName = GetByFixNameId(fix.FixNameId);
            wsObject.Name = fixName?.Description;
            wsObject.Category = "";
            wsObject.Mileage =_diagnosticReportService.GetNextServiceMileageInterval(mileage, sdkObject.Mileage, sdkObject.MileageRepeat);
            if (fixName != null)
            {
                wsObject.ServiceInfo = _diagnosticReportService.GetWebServiceObjectFixInfo(fix, fixName, sdkObject);
            }
            return wsObject;
        }

        public Fix GetFixByServiceId(string serviceId)
        {
            return _context.ScheduleMaintenanceServices
                         .Join(_context.Fixes, scheduleMaintenanceService => scheduleMaintenanceService.FixNameId, B => B.FixNameId, (scheduleMaintenanceService, B) => new { scheduleMaintenanceService, B })
                         .Where(e => e.scheduleMaintenanceService.ScheduleMaintenanceServiceId.Equals(serviceId)).Select(e => e.B).FirstOrDefault();
        }

        public FixName GetByFixNameId(string fixNameId)
        {
            return _context.FixNames.Where(x => x.FixNameId == fixNameId).FirstOrDefault();
        }

    }
}
