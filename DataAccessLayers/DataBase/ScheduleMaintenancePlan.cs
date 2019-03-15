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
    
    public partial class ScheduleMaintenancePlan
    {
        public string ScheduleMaintenancePlanId { get; set; }
        public Nullable<int> Type { get; set; }
        public string ManufacturerName { get; set; }
        public string Name { get; set; }
        public Nullable<int> MaxMileage { get; set; }
        public string AdminUserIdCreatedBy { get; set; }
        public string AdminUserIdUpdatedBy { get; set; }
        public Nullable<bool> HasEngineTypeDefined { get; set; }
        public string EngineTypesString { get; set; }
        public Nullable<bool> HasEngineVINCodeDefined { get; set; }
        public string EngineVINCodesString { get; set; }
        public Nullable<bool> HasYearDefined { get; set; }
        public string YearsString { get; set; }
        public Nullable<bool> HasMakeDefined { get; set; }
        public string MakesString { get; set; }
        public Nullable<bool> HasModelDefined { get; set; }
        public string ModelsString { get; set; }
        public Nullable<bool> HasTrimLevelDefined { get; set; }
        public string TrimLevelsString { get; set; }
        public Nullable<bool> HasTransmissionDefined { get; set; }
        public string TransmissionsString { get; set; }
        public Nullable<bool> IsForHistoricalVehicles { get; set; }
        public Nullable<System.DateTime> CreatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
    }
}