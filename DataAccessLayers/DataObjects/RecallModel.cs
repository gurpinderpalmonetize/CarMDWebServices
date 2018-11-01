using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
   public class RecallModel
    {
        public int RecordNumber { get; set; }
        public string CampaignNumber { get; set; }
        public string RecallDate { get; set; }
        public string DefectDescription { get; set; }
        public string DefectDescription_es { get; set; }
        public string DefectDescription_fr { get; set; }
        public string DefectDescription_zh { get; set; }
        public string DefectConsequence { get; set; }
        public string DefectConsequence_es { get; set; }
        public string DefectConsequence_fr { get; set; }
        public string DefectConsequence_zh { get; set; }
        public string DefectCorrectiveAction { get; set; }
        public string DefectCorrectiveAction_es { get; set; }
        public string DefectCorrectiveAction_fr { get; set; }
        public string DefectCorrectiveAction_zh { get; set; }
    }
}
