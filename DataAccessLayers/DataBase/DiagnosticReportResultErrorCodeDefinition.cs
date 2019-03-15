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
    
    public partial class DiagnosticReportResultErrorCodeDefinition
    {
        public System.Guid DiagnosticReportResultErrorCodeDefinitionId { get; set; }
        public System.Guid DiagnosticReportResultErrorCodeId { get; set; }
        public string Title { get; set; }
        public string Conditions { get; set; }
        public string PossibleCauses { get; set; }
        public int Trips { get; set; }
        public string MessageIndicatorLampFile { get; set; }
        public string TransmissionControlIndicatorLampFile { get; set; }
        public string PassiveAntiTheftIndicatorLampFile { get; set; }
        public string ServiceThrottleSoonIndicatorLampFile { get; set; }
        public string MonitorType { get; set; }
        public string MonitorFile { get; set; }
        public Nullable<System.Guid> DTCMasterCodeId { get; set; }
        public Nullable<System.Guid> DTCCodeId { get; set; }
        public Nullable<System.Guid> VehicleTypeCodeId { get; set; }
        public Nullable<System.Guid> VehicleTypeId { get; set; }
        public string ManufacturerName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string BodyCode { get; set; }
        public string EngineType { get; set; }
        public string EngineVINCode { get; set; }
        public string EngineTypeVINLookup { get; set; }
        public string TransmissionControlType { get; set; }
        public string LaymansTermTitle { get; set; }
        public string LaymansTermConditions { get; set; }
        public string Title_es { get; set; }
        public string Title_fr { get; set; }
        public string Title_zh { get; set; }
        public string Conditions_es { get; set; }
        public string Conditions_fr { get; set; }
        public string Conditions_zh { get; set; }
        public string PossibleCauses_es { get; set; }
        public string PossibleCauses_fr { get; set; }
        public string PossibleCauses_zh { get; set; }
        public string LaymansTermTitle_es { get; set; }
        public string LaymansTermTitle_fr { get; set; }
        public string LaymansTermTitle_zh { get; set; }
        public string LaymansTermConditions_es { get; set; }
        public string LaymansTermConditions_fr { get; set; }
        public string LaymansTermConditions_zh { get; set; }
        public int LaymansTermSeverityLevel { get; set; }
        public string LaymansTermEffectOnVehicle { get; set; }
        public string LaymansTermEffectOnVehicle_es { get; set; }
        public string LaymansTermEffectOnVehicle_fr { get; set; }
        public string LaymansTermEffectOnVehicle_zh { get; set; }
        public string LaymansTermResponsibleComponentOrSystem { get; set; }
        public string LaymansTermResponsibleComponentOrSystem_es { get; set; }
        public string LaymansTermResponsibleComponentOrSystem_fr { get; set; }
        public string LaymansTermResponsibleComponentOrSystem_zh { get; set; }
        public string LaymansTermWhyItsImportant { get; set; }
        public string LaymansTermWhyItsImportant_es { get; set; }
        public string LaymansTermWhyItsImportant_fr { get; set; }
        public string LaymansTermWhyItsImportant_zh { get; set; }
        public string LaymansTermSeverityLevelDefinition { get; set; }
        public string LaymansTermSeverityLevelDefinition_es { get; set; }
        public string LaymansTermSeverityLevelDefinition_fr { get; set; }
        public string LaymansTermSeverityLevelDefinition_zh { get; set; }
        public string LaymansTermDescription { get; set; }
        public string LaymansTermDescription_es { get; set; }
        public string LaymansTermDescription_fr { get; set; }
        public string LaymansTermDescription_zh { get; set; }
    }
}