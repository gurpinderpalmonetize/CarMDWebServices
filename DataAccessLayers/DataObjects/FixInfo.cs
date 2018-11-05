using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
   public class FixInfo
    {
   
        public Guid FixId { get; set; }
        public Guid? FixNameId { get; set; }
        public string Name { get; set; }
        public string ErrorCode { get; set; }
        public int ErrorCodeSystemType { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public decimal LaborHours { get; set; }
        public decimal LaborRate { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalCost;
        public int FixRating = 0;
        public int FrequencyCount { get; set; }
        public int CommunityVotes { get; set; }
        public bool OBDFixApproved { get; set; }
        public decimal PredictiveDiagnosticPercentInMileage { get; set; }

        public int PredictiveDiagnosticCount { get; set; }
      //  public FixFeedbackInfo[] FixFeedbacks { get; set; }
       // public ArticleInfo[] RelatedArticles { get; set; }
        //public FixPartInfo[] FixParts;
    }
}
