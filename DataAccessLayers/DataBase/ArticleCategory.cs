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
    
    public partial class ArticleCategory
    {
        public string ArticleCategoryId { get; set; }
        public string ArticleCategoryIdContainer { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsCarMD { get; set; }
        public Nullable<bool> IsCanOBD2 { get; set; }
        public Nullable<bool> IsOBDFix { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> CreatedDateTimeUTC { get; set; }
    }
}