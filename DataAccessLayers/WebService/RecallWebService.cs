using DataAccessLayers.DataBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataAccessLayers.WebService
{
    public class RecallWebService
    {
        public static int Search(int year, string make, string model, string trimLevel)
        {
            var recallCollection = new List<RecallCollection>();
            using (innovaEntities dbContext = new innovaEntities())
            {
               var recall = (from recalls in dbContext.Recalls
                                    where
                                     recalls.Make != string.Empty && recalls.Make == make
                                     && recalls.Model != string.Empty && recalls.Model == model
                                     && recalls.Year != null && recalls.Year == year
                                    select new RecallCollection
                                    {
                                        RecordNumber = recalls.RecordNumber,
                                        CampaignNumber = recalls.CampaignNumber,
                                        RecallDate = recalls.RecallDate, //DateTime.ParseExact(recallCollection.RecallDate, "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
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

                //if (!string.IsNullOrEmpty(recallCollection.))
                //{
                //    DateTime recallDate = DateTime.MinValue;
                //    recallDate = DateTime.ParseExact(recallCollection.RecallDate, "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
                //    recallCollection.RecallDate = recallDate.ToLongDateString();
                //}


                recallCollection.Add(recall);

                return recallCollection.Count();
            }
        }

        public static RecallCollection GetByYearMakeModelDateRange(int? year, string make, string model, string startDate = null, string endDate = null)
        {
            var recallCollection = new RecallCollection();
            using (innovaEntities dbContext = new innovaEntities())
            {
                recallCollection = (from recalls in dbContext.Recalls
                                    where
                                     recalls.Make != string.Empty && recalls.Make == make
                                     && recalls.Model != string.Empty && recalls.Model == model
                                     && recalls.Year != null && recalls.Year == year
                                     && recalls.RecallDate != null && recalls.RecallDate == startDate
                                     && recalls.RecallDate != null && recalls.RecallDate == endDate
                                    //(Recall.RecallDate >= @StartDateString 
                                    //(Recall.RecallDate <= @EndDateString 

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

