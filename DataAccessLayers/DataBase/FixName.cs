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
    
    public partial class FixName
    {
        public string FixNameId { get; set; }
        public string FixNameIdChiltonComponentLocator { get; set; }
        public string FixNameIdChiltonWiringDiagrams { get; set; }
        public Nullable<int> FixType { get; set; }
        public Nullable<int> FixRating { get; set; }
        public string Description { get; set; }
        public string Description_es { get; set; }
        public string Description_fr { get; set; }
        public string Description_zh { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> EnableForSchedMaint { get; set; }
        public Nullable<bool> EnableForUnSchedMaint { get; set; }
        public Nullable<int> DefaultUnschedMaintMileage { get; set; }
        public Nullable<int> DefaultUnschedMaintMileageRepeat { get; set; }
        public string CreatedByAdminUserId { get; set; }
        public string UpdatedByAdminUserId { get; set; }
        public Nullable<System.DateTime> CreatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
    }
}
