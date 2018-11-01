using DataAccessLayers.DataObjects;
using DataAccessLayers.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.WebService
{
    public class RecallWebService
    {
        public static RecallCollection Search(int year, string make, string model, string trimLevel)
        {
            var recallCollection = new RecallCollection();
            using (innovadev01Entities dbContext = new innovadev01Entities())
            {
                recallCollection = (from recalls in dbContext.Recalls
                                    join recall_ByCleanModel in dbContext.Recall_ByCleanModel
                                    on recalls.RecordNumber equals recall_ByCleanModel.RecordNumber
                                    where
                                     recalls.Make != string.Empty && recalls.Make == make
                                     && recalls.Model != string.Empty && recalls.Model == model
                                     && recalls.Year != null && recalls.Year == year
                                    select new RecallCollection
                                    {
                                        RecordNumber = recalls.RecordNumber,
                                        CampaignNumber = recalls.CampaignNumber,
                                        RecallDate = recalls.RecallDate,
                                        DefectDescription_es = recalls.DefectDescription_es,
                                        DefectDescription_fr = recalls.DefectDescription_fr,
                                        DefectDescription_zh = recalls.DefectDescription_zh,
                                        DefectConsequence = recalls.DefectConsequence,
                                        DefectConsequence_es = recalls.DefectCorrectiveAction_es,
                                        DefectConsequence_fr = recalls.DefectCorrectiveAction_fr,
                                        DefectConsequence_zh = recalls.DefectCorrectiveAction_zh,
                                        DefectCorrectiveAction = recalls.DefectCorrectiveAction,
                                        DefectCorrectiveAction_es = recalls.DefectConsequence_es,
                                        DefectCorrectiveAction_fr = recalls.DefectConsequence_fr,
                                        DefectCorrectiveAction_zh = recalls.DefectConsequence_zh,
                                    }).Distinct().OrderByDescending(x => x.RecallDate).FirstOrDefault();
                if (!string.IsNullOrEmpty(recallCollection.RecallDate))
                {
                    DateTime recallDate = DateTime.MinValue;
                    recallDate = DateTime.ParseExact(recallCollection.RecallDate, "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
                    recallCollection.RecallDate = recallDate.ToLongDateString();

                }

                return recallCollection;
            }
        }

        public static RecallCollection GetByYearMakeModelDateRange(int? year, string make, string model, string startDate = null, string endDate = null)
        {
            var recallCollection = new RecallCollection();
            using (innovadev01Entities dbContext = new innovadev01Entities())
            {
                recallCollection = (from recalls in dbContext.Recalls
                                    join recall_ByCleanModel in dbContext.Recall_ByCleanModel
                                    on recalls.RecordNumber equals recall_ByCleanModel.RecordNumber
                                    where
                                     recalls.Make != string.Empty && recalls.Make == make
                                     && recalls.Model != string.Empty && recalls.Model == model
                                     && recalls.Year != null && recalls.Year == year

                                    select new RecallCollection
                                    {
                                        RecallDate = recalls.RecallDate,
                                        DefectDescription_es = recalls.DefectDescription_es,
                                        DefectDescription_fr = recalls.DefectDescription_fr,
                                        DefectDescription_zh = recalls.DefectDescription_zh,
                                        DefectConsequence = recalls.DefectConsequence,
                                        DefectConsequence_es = recalls.DefectCorrectiveAction_es,
                                        DefectConsequence_fr = recalls.DefectCorrectiveAction_fr,
                                        DefectConsequence_zh = recalls.DefectCorrectiveAction_zh,
                                        DefectCorrectiveAction = recalls.DefectCorrectiveAction,
                                        DefectCorrectiveAction_es = recalls.DefectConsequence_es,
                                        DefectCorrectiveAction_fr = recalls.DefectConsequence_fr,
                                        DefectCorrectiveAction_zh = recalls.DefectConsequence_zh,
                                    }).Distinct().GroupBy(x => x.CampaignNumber).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.RecallDate).FirstOrDefault();
                if (!string.IsNullOrEmpty(recallCollection.RecallDate))
                {
                    DateTime recallDate = DateTime.MinValue;
                    recallDate = DateTime.ParseExact(recallCollection.RecallDate, "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
                    recallCollection.RecallDate = recallDate.ToLongTimeString();
                }

                return recallCollection;
            }
        }
    }
}

