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
    
    public partial class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Description { get; set; }
        public string RoleId { get; set; }
        public System.DateTime CreatedDateTimeUTC { get; set; }
        public System.DateTime UpdatedDateTimeUTC { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
