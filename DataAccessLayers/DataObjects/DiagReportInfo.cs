namespace DataAccessLayers.DataObjects
{
    public class DiagReportInfo
    {
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
