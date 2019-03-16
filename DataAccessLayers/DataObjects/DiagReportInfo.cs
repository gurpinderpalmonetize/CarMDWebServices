namespace DataAccessLayers.DataObjects
{
    public class DiagReportInfo
    {
        public WebServiceSessionStatus WebServiceSessionStatus { get; set; }
        public VehicleInfo Vehicle { get; set; }
        public FixInfo[] FixInfo { get; set; }
        public SymptomInfo[] Symptoms { get; set; }
        public ErrorCodeInfo[] Errors { get; set; }
        public FreezeFrameInfo[] FreezeFrame { get; set; }
        public MonitorInfo[] Monitors { get; set; }
        public FixInfo[] Fixes { get; set; }
        public RecallInfo[] Recalls { get; set; }
        public TSBInfo[] TSBs { get; set; }
        public TSBCategoryInfo[] TSBCategories { get; set; }
        public string ToolLEDStatusDesc { get; set; }
        public int? TSBCountAll { get; set; }
        public ScheduleMaintenanceServiceInfo[] ScheduleMaintenanceServices { get; set; }
        public ScheduleMaintenanceServiceInfo[] UnScheduledMaintenanceServices { get; set; }
        public VehicleWarrantyDetailInfo[] VehicleWarrantyDetails { get; set; }
        public FixStatusInfo FixStatusInfo { get; set; }

        public long DiagnosticReportId { get; set; }
        public bool? ScheduledMaintenanceNextMileage { get; set; }
        public bool? HasScheduledMaintenance { get; set; }
        public bool? HasUnScheduledMaintenance { get; set; }
        public bool? HasVehicleWarrantyDetails { get; set; }
        public bool? UnScheduledMaintenanceNextMileage { get; set; }
        public bool IsValid { get; set; }
        public ValidationFailureModel[] ValidationFailures { get; set; }

    }

}
