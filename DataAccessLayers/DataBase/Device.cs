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
    
    public partial class Device
    {
        public string DeviceId { get; set; }
        public string UserId { get; set; }
        public string ChipId { get; set; }
        public Nullable<int> MaximumVehicleCount { get; set; }
        public Nullable<int> MaximumUploadCount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsPrimaryOwner { get; set; }
        public Nullable<System.DateTime> CreatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
        public string UserRequestingTransferId { get; set; }
        public Nullable<System.DateTime> UserRequestDateTimeUTC { get; set; }
        public Nullable<System.DateTime> TransferAcceptedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> DeactivatedDateTimeUTC { get; set; }
        public Nullable<bool> IsManualDevice { get; set; }
        public Nullable<int> TransferCount { get; set; }
    }
}