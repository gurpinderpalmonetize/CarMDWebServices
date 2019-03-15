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
    
    public partial class Article
    {
        public string ArticleId { get; set; }
        public string ArticleCategoryIdPrimary { get; set; }
        public string URLName { get; set; }
        public string AdminUserIdCreated { get; set; }
        public string AdminUserIdUpdated { get; set; }
        public string AdminUserIdLastReviewed { get; set; }
        public Nullable<int> ArticleType { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Keywords { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Author { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string VideoUrl { get; set; }
        public string VideoThumbnailUrl { get; set; }
        public Nullable<int> VideoDurationSeconds { get; set; }
        public Nullable<int> VideoWidth { get; set; }
        public Nullable<int> VideoHeight { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsCarMD { get; set; }
        public Nullable<bool> IsCanOBD2 { get; set; }
        public Nullable<bool> IsOBDFix { get; set; }
        public Nullable<bool> IsFree { get; set; }
        public Nullable<System.DateTime> LastReviewedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> CreatedDateTimeUTC { get; set; }
        public Nullable<System.DateTime> UpdatedDateTimeUTC { get; set; }
    }
}