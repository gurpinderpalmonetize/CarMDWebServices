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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DiagnosticReport
    {
        public long DiagnosticReportId { get; set; }
        public string DiagnosticReportResultId { get; set; }
        public string UserId { get; set; }
        public bool HasMasterTechsAssigned { get; set; }
        public string MasterTechsAssignedIdList { get; set; }
        public Nullable<System.DateTime> MasterTechProvideFixFeedbackByOverrideDateTimeUTC { get; set; }
        public string AdminUserId_PwrWorkingOnFix { get; set; }
        public Nullable<System.DateTime> PwrAdminUserWorkingOnFixAssignedDateTimeUTC { get; set; }
        public string AdminUserId_AbsWorkingOnFix { get; set; }
        public Nullable<System.DateTime> AbsAdminUserWorkingOnFixAssignedDateTimeUTC { get; set; }
        public string AdminUserId_SrsWorkingOnFix { get; set; }
        public Nullable<System.DateTime> SrsAdminUserWorkingOnFixAssignedDateTimeUTC { get; set; }
        public string VehicleId { get; set; }
        public string DeviceId { get; set; }
        public string CarScanSessionId { get; set; }
        public string DiagnosticUploadId { get; set; }
        public string OrderLineItemId { get; set; }
        public string DiagnosticReportPreliminaryDiagnosisId { get; set; }
        public string RepairOrderNumber { get; set; }
        public int VehicleMileage { get; set; }
        public int Market { get; set; }
        public int Language { get; set; }
        public int Currency { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public int PwrDiagnosticReportFixStatusWhenCreated { get; set; }
        public int Obd1DiagnosticReportFixStatusWhenCreated { get; set; }
        public int AbsDiagnosticReportFixStatusWhenCreated { get; set; }
        public int SrsDiagnosticReportFixStatusWhenCreated { get; set; }
        public int PwrDiagnosticReportFixStatus { get; set; }
        public string PwrDiagnosticReportFixCancelReason { get; set; }
        public int Obd1DiagnosticReportFixStatus { get; set; }
        public string Obd1DiagnosticReportFixCancelReason { get; set; }
        public int AbsDiagnosticReportFixStatus { get; set; }
        public string AbsDiagnosticReportFixCancelReason { get; set; }
        public int SrsDiagnosticReportFixStatus { get; set; }
        public string SrsDiagnosticReportFixCancelReason { get; set; }
        public Nullable<System.DateTime> PwrDiagnosticReportFixStatusClosedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> Obd1DiagnosticReportFixStatusClosedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> AbsDiagnosticReportFixStatusClosedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> SrsDiagnosticReportFixStatusClosedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> PwrFixNotFoundFixPromisedByDateTimeUTC { get; set; }
        public Nullable<System.DateTime> Obd1FixNotFoundFixPromisedByDateTimeUTC { get; set; }
        public Nullable<System.DateTime> AbsFixNotFoundFixPromisedByDateTimeUTC { get; set; }
        public Nullable<System.DateTime> SrsFixNotFoundFixPromisedByDateTimeUTC { get; set; }
        public Nullable<System.DateTime> NoFixProcessCompletedAndSentDateTimeUTC { get; set; }
        public string RawUploadString { get; set; }
        public string RawFreezeFrameDataString { get; set; }
        public string RawMonitorsDataString { get; set; }
        public int ToolTypeFormat { get; set; }
        public string SoftwareVersion { get; set; }
        public string FirmwareVersion { get; set; }
        public string VIN { get; set; }
        public string PwrMilCode { get; set; }
        public string PwrStoredCodesString { get; set; }
        public string PwrPendingCodesString { get; set; }
        public string Obd1StoredCodesString { get; set; }
        public string Obd1PendingCodesString { get; set; }
        public string AbsStoredCodesString { get; set; }
        public string AbsPendingCodesString { get; set; }
        public string SrsStoredCodesString { get; set; }
        public string SrsPendingCodesString { get; set; }
        public string EnhancedDtcsString { get; set; }
        public Nullable<int> ToolLEDStatus { get; set; }
        public Nullable<int> ToolMilStatus { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
        public System.DateTime CreatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> RequestedTechnicianContactDateTimeUTC { get; set; }
        public string RequestedTechnicianContactComments { get; set; }
        public bool IsManualReport { get; set; }
        public int ManualDiagnosticReportType { get; set; }
        public bool IsPwrObd1FixFeedbackRequired { get; set; }
        public bool IsPwrObd2FixFeedbackRequired { get; set; }
        public bool IsAbsFixFeedbackRequired { get; set; }
        public bool IsSrsFixFeedbackRequired { get; set; }
        public Nullable<System.DateTime> MasterTechNotificationsSentDateTimeUTC { get; set; }
        public Nullable<bool> PwrIsFixNotificationRequested { get; set; }
        public Nullable<bool> AbsIsFixNotificationRequested { get; set; }
        public Nullable<bool> SrsIsFixNotificationRequested { get; set; }
        public Nullable<int> PwrDiagnosticReportFixFeedbackStatus { get; set; }
        public Nullable<int> Obd1DiagnosticReportFixFeedbackStatus { get; set; }
        public Nullable<int> AbsDiagnosticReportFixFeedbackStatus { get; set; }
        public Nullable<int> SrsDiagnosticReportFixFeedbackStatus { get; set; }
        public Nullable<System.DateTime> FixProvidedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> WhatFixedMyCarEmailSentDateTimeUTC { get; set; }
        public Nullable<System.DateTime> PastDueEmailSentDateTimeUTC { get; set; }
        public string SymptomId { get; set; }
        public string PwrPermanentCodesString { get; set; }
        public int SoftwareType { get; set; }
        public string ParentDiagnosticReportId { get; set; }
        public string ManualRawFreezeFrameDataString { get; set; }
        public bool AdditionalHelpRequired { get; set; }
        public Nullable<bool> IsNotifiedRequester { get; set; }
        public Nullable<System.DateTime> NotifiedRequesterDateTimeUTC { get; set; }
        public string NotifiedRequesterVia { get; set; }
        public string Note { get; set; }
        public string Old_DiagnosticReportId { get; set; }

        [ForeignKey("VehicleId")]
        [Required]
        public Vehicle Vehicle { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }

        [ForeignKey("DeviceId")]
        public Device Device { get; set; }

        [ForeignKey("SymptomId")]
        public Symptom Symptom { get; set; }
    }
}
