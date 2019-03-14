using DataAccessLayers.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class RecallInfo
    {
        public RecallInfo()
        {
        }
        public int RecordNumber { get; set; }

        public string CampaignNumber { get; set; }

        public string RecallDateString { get; set; }

        public string DefectDescription { get; set; }

        public string DefectConsequence { get; set; }
        public string DefectCorrectiveAction { get; set; }

        protected internal static RecallInfo GetWebServiceObject(Recall sdkObject)
        {
            RecallInfo wsObject = new RecallInfo();

            //wsObject.CampaignNumber = sdkObject.CampaignNumber;
            //wsObject.DefectConsequence = sdkObject.DefectConsequence_Translated;
            //wsObject.DefectCorrectiveAction = sdkObject.DefectCorrectiveAction_Translated;
            //wsObject.DefectDescription = sdkObject.DefectDescription_Translated;

            //wsObject.RecallDateString = sdkObject.RecallDate.ToShortDateString();
            //wsObject.RecordNumber = sdkObject.RecordNumber;
            return wsObject;

        }
    }
}
