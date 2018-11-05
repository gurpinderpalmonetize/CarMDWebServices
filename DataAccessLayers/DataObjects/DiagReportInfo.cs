using DataAccessLayers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class DiagReportInfo
    {
        public string DiagnosticReportId { get; set; }
        public bool? ScheduledMaintenanceNextMileage { get; set; }
        public bool? HasScheduledMaintenance { get; set; }
        public bool? HasUnScheduledMaintenance { get; set; }
        public bool? HasVehicleWarrantyDetails { get; set; }
        public bool? UnScheduledMaintenanceNextMileage { get; set; }
        public bool IsValid { get; set; }
        public ValidationFailureModel[] ValidationFailures { get; set; }

    }

}
