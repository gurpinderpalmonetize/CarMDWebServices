//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayers.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TSB
    {
        public Nullable<int> TSBID { get; set; }
        public string Description { get; set; }
        public string FileNamePDF { get; set; }
        public string ManufacturerNumber { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string TSBText { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        public int id { get; set; }
    }
}
