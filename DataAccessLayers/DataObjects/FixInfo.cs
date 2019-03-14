using System;

namespace DataAccessLayers.DataObjects
{
    public class FixInfo
    {
        public string FixId { get; set; }
        public string FixNameId { get; set; }
        public string Name { get; set; }
        public string ErrorCode { get; set; }
        public int ErrorCodeSystemType { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public decimal? LaborHours { get; set; }
        public decimal? LaborRate { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? AdditionalCost { get; set; }
        public decimal? TotalCost;
        public int FixRating = 0;
        public int FrequencyCount { get; set; }
        public int CommunityVotes { get; set; }
        public bool OBDFixApproved { get; set; }
        public decimal PredictiveDiagnosticPercentInMileage { get; set; }
        public int PredictiveDiagnosticCount { get; set; }
        public FixFeedbackInfo[] FixFeedbacks { get; set; }
        public ArticleInfo[] RelatedArticles { get; set; }
        public FixPartInfo[] FixParts;
    }

    public class FixFeedbackInfo
    {
        public int DiagnosticReportId { get; set; }
        public bool IsReportValid { get; set; } = true;
        public string CouldNotFixReason { get; set; } = "";
        public string PrimaryErrorCode { get; set; } = "";
        public string DiagnosticReportErrorCodeSystemType { get; set; } = "";
        public string Fix { get; set; } = "";
        public int AverageDiagnosticTimeMinutes { get; set; } = 0;
        public int FrequencyEncountered { get; set; } = 0;
        public string FixDifficultyRating { get; set; } = "";
        public string ErrorCodesThatApply { get; set; } = "";
        public string TechComments { get; set; } = "";
        public string BasicToolsRequired { get; set; } = "";
        public string SpecialtyToolsRequired { get; set; } = "";
        public string TipsAndTricks { get; set; } = "";
        public FixFeedbackPartInfo[] Parts { get; set; }
    }

    public class ArticleInfo
    {
        public Guid ArticleId { get; set; }
        public string AdminUserNameCreated { get; set; }
        public string AdminUserNameUpdated { get; set; }
        public Guid PrimaryArticleCategoryId { get; set; }
        public string PrimaryArticleCategoryName { get; set; }
        public string ArticleTypeName { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string CreatedDateTimeUTCString { get; set; }
        public string DateString { get; set; }
        public string EndDateString { get; set; }
        public bool IsActive { get; set; }
        public bool IsFree { get; set; }
        public string Keywords { get; set; }
        public string StartDateString { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }
        public string UpdatedDateTimeUTCString { get; set; }
        public int VideoDurationSeconds { get; set; }
        public int VideoHeight { get; set; }
        public string VideoThumbnailUrl { get; set; }
        public string VideoDownloadUrl { get; set; }
        public string VideoStreamingUrl { get; set; }
        public int VideoWidth { get; set; }
    }

    public class FixPartInfo
    {
        public string ACESPartTypeID = "";
        public int Quantity = 0;
        public string ManufacturerName = "";
        public string MakesList = "";
        public string PartNumber = "";
        public string NapaPartNumber = "";
        public string Name = "";
        public string Description = "";
        public decimal Price = 0;
        public string CodemasterID = "";
        public FixPartOemInfo[] FixPartOemInfos;
    }

    public class FixFeedbackPartInfo
    {
        public Guid DiagnosticReportFixFeedbackId { get; set; }
        public string PartName { get; set; } = "";
        public string PartNumber { get; set; } = "";
    }

    public class FixPartOemInfo
    {
        public string retailer { get; set; } = "";
        public string manufacturer { get; set; } = "";
        public string oemPartNumber { get; set; } = "";
    }
}
