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
    
    public partial class FixPart
    {
        public string FixPartId { get; set; }
        public string FixId { get; set; }
        public string PartId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string CodemasterID { get; set; }
        public Nullable<decimal> PricePending { get; set; }
    }
}
