//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayers.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class VehiclesWithScheduledMaintenanceRemindersView
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<int> Year { get; set; }
        public string EngineType { get; set; }
        public string EngineVINCode { get; set; }
        public string TrimLevel { get; set; }
        public string TransmissionControlType { get; set; }
        public string VehicleId { get; set; }
        public string UserId { get; set; }
        public Nullable<int> Mileage { get; set; }
        public Nullable<System.DateTime> ScheduleMaintenanceAlertLastSentDateTimeUTC { get; set; }
        public string UserTypeExternalId { get; set; }
        public Nullable<int> UserType { get; set; }
    }
}
