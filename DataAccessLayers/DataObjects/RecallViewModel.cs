using DataAccessLayers.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class RecallViewModel
    {
        private string campaignNumber = "";
        private string defectDescription = "";
        private string defectDescription_es = "";
        private string defectDescription_fr = "";
        private string defectDescription_zh = "";
        private string defectConsequence = "";
        private string defectConsequence_es = "";
        private string defectConsequence_fr = "";
        private string defectConsequence_zh = "";
        private string defectCorrectiveAction = "";
        private string defectCorrectiveAction_es = "";
        private string defectCorrectiveAction_fr = "";
        private string defectCorrectiveAction_zh = "";
        private int recordNumber;
        private DateTime recallDate;

        public RecallViewModel(RecallCollection recall)
        {
            this.recordNumber = recall.RecordNumber;
            this.campaignNumber = recall.CampaignNumber;
          //  this.recallDate = recall.RecallDate;
            this.defectDescription = recall.DefectConsequence;
            //this.defectDescription_es = defectDescription_es;
            //this.defectDescription_fr = defectDescription_fr;
            //this.defectDescription_zh = defectDescription_zh;
            //this.defectConsequence = defectConsequence;
            //this.defectConsequence_es = defectConsequence_es;
            //this.defectConsequence_fr = defectConsequence_fr;
            //this.defectConsequence_zh = defectConsequence_zh;
            //this.defectCorrectiveAction = defectCorrectiveAction;
            //this.defectCorrectiveAction_es = defectCorrectiveAction_es;
            //this.defectCorrectiveAction_fr = defectCorrectiveAction_fr;
            //this.defectCorrectiveAction_zh = defectCorrectiveAction_zh;
        }

        public int RecordNumber
        {
            get
            {
                return this.recordNumber;
            }
            set
            {
                this.recordNumber = value;
            }
        }

        public string CampaignNumber
        {
            get
            {
                return this.campaignNumber;
            }
            set
            {
                this.campaignNumber = value;
            }
        }

        public DateTime RecallDate
        {
            get
            {
                return this.recallDate;
            }
            set
            {
                this.recallDate = value;
            }
        }

        public string DefectDescription
        {
            get
            {
                return this.defectDescription;
            }
            set
            {
                this.defectDescription = value;
            }
        }

        public string DefectDescription_es
        {
            get
            {
                return this.defectDescription_es;
            }
            set
            {
                this.defectDescription_es = value;
            }
        }

        public string DefectDescription_fr
        {
            get
            {
                return this.defectDescription_fr;
            }
            set
            {
                this.defectDescription_fr = value;
            }
        }

        public string DefectDescription_zh
        {
            get
            {
                return this.defectDescription_zh;
            }
            set
            {
                this.defectDescription_zh = value;
            }
        }


        public string DefectConsequence
        {
            get
            {
                return this.defectConsequence;
            }
            set
            {
                this.defectConsequence = value;
            }
        }

        public string DefectConsequence_es
        {
            get
            {
                return this.defectConsequence_es;
            }
            set
            {
                this.defectConsequence_es = value;
            }
        }

        public string DefectConsequence_fr
        {
            get
            {
                return this.defectConsequence_fr;
            }
            set
            {
                this.defectConsequence_fr = value;
            }
        }

        public string DefectConsequence_zh
        {
            get
            {
                return this.defectConsequence_zh;
            }
            set
            {
                this.defectConsequence_zh = value;
            }
        }

        public string DefectCorrectiveAction
        {
            get
            {
                return this.defectCorrectiveAction;
            }
            set
            {
                this.defectCorrectiveAction = value;
            }
        }

        public string DefectCorrectiveAction_es
        {
            get
            {
                return this.defectCorrectiveAction_es;
            }
            set
            {
                this.defectCorrectiveAction_es = value;
            }
        }

        public string DefectCorrectiveAction_fr
        {
            get
            {
                return this.defectCorrectiveAction_fr;
            }
            set
            {
                this.defectCorrectiveAction_fr = value;
            }
        }

        public string DefectCorrectiveAction_zh
        {
            get
            {
                return this.defectCorrectiveAction_zh;
            }
            set
            {
                this.defectCorrectiveAction_zh = value;
            }
        }


      //  public static RecallCollection GetByRecordNumberXmlList(Registry registry, string recordNumberXmlList)
      //  {
      //      RecallCollection recallCollection = new RecallCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_LoadByRecordNumberXmlList";
      //          dataReaderWrapper.AddNVarChar("RecordNumbersXml", recordNumberXmlList);
      //          dataReaderWrapper.Execute();
      //          while (dataReaderWrapper.Read())
      //          {
      //              Recall recall = new Recall(registry, dataReaderWrapper.GetInt32("RecordNumber"), dataReaderWrapper.GetString("CampaignNumber"), DateTime.ParseExact(dataReaderWrapper.GetString("RecallDate"), "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat), dataReaderWrapper.GetString("DefectDescription"), dataReaderWrapper.GetString("DefectDescription_es"), dataReaderWrapper.GetString("DefectDescription_fr"), dataReaderWrapper.GetString("DefectDescription_zh"), dataReaderWrapper.GetString("DefectConsequence"), dataReaderWrapper.GetString("DefectConsequence_es"), dataReaderWrapper.GetString("DefectConsequence_fr"), dataReaderWrapper.GetString("DefectConsequence_zh"), dataReaderWrapper.GetString("DefectCorrectiveAction"), dataReaderWrapper.GetString("DefectCorrectiveAction_es"), dataReaderWrapper.GetString("DefectCorrectiveAction_fr"), dataReaderWrapper.GetString("DefectCorrectiveAction_zh"));
      //              recallCollection.Add(recall);
      //          }
      //      }
      //      return recallCollection;
      //  }

      //  public static int GetRecallCount(Registry registry, int year, string make, string model, string trimLevel)
      //  {
      //      int num = 0;
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_Count";
      //          dataReaderWrapper.AddInt32("Year", year);
      //          dataReaderWrapper.AddNVarChar("Make", make);
      //          dataReaderWrapper.AddNVarChar("Model", model);
      //          dataReaderWrapper.AddNVarChar("Trim", trimLevel);
      //          dataReaderWrapper.Execute();
      //          if (dataReaderWrapper.Read())
      //              num = dataReaderWrapper.GetInt32("RecordCount");
      //      }
      //      return num;
      //  }

      //  public static RecallCollection GetByYearMakeModelDateRange(Registry registry, int? year, string makesXmlList, string model, DateTime? startDate, DateTime? endDate)
      //  {
      //      RecallCollection recallCollection = new RecallCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper1 = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper1.ProcedureName = "Recall_LoadByYearMakeModelDateRange";
      //          if (year.HasValue && year.Value > 0)
      //              dataReaderWrapper1.AddInt32("Year", year.Value);
      //          if (!string.IsNullOrEmpty(makesXmlList))
      //              dataReaderWrapper1.AddNVarChar("MakesXmlList", makesXmlList);
      //          if (!string.IsNullOrEmpty(model))
      //              dataReaderWrapper1.AddNVarChar("Model", model);
      //          DateTime dateTime;
      //          int num;
      //          if (startDate.HasValue)
      //          {
      //              SqlDataReaderWrapper dataReaderWrapper2 = dataReaderWrapper1;
      //              string parameterName = "StartDateString";
      //              dateTime = startDate.Value;
      //              num = dateTime.Year;
      //              string str1 = num.ToString();
      //              dateTime = startDate.Value;
      //              num = dateTime.Month;
      //              string str2 = num.ToString("00");
      //              dateTime = startDate.Value;
      //              num = dateTime.Day;
      //              string str3 = num.ToString("00");
      //              string parameterValue = str1 + str2 + str3;
      //              dataReaderWrapper2.AddNVarChar(parameterName, parameterValue);
      //          }
      //          if (endDate.HasValue)
      //          {
      //              SqlDataReaderWrapper dataReaderWrapper2 = dataReaderWrapper1;
      //              string parameterName = "EndDateString";
      //              dateTime = endDate.Value;
      //              num = dateTime.Year;
      //              string str1 = num.ToString();
      //              dateTime = endDate.Value;
      //              num = dateTime.Month;
      //              string str2 = num.ToString("00");
      //              dateTime = endDate.Value;
      //              num = dateTime.Day;
      //              string str3 = num.ToString("00");
      //              string parameterValue = str1 + str2 + str3;
      //              dataReaderWrapper2.AddNVarChar(parameterName, parameterValue);
      //          }
      //          dataReaderWrapper1.Execute();
      //          while (dataReaderWrapper1.Read())
      //          {
      //              DateTime recallDate = DateTime.MinValue;
      //              if (dataReaderWrapper1.GetString("RecallDate") != "")
      //              {
      //                  try
      //                  {
      //                      recallDate = DateTime.ParseExact(dataReaderWrapper1.GetString("RecallDate"), "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
      //                  }
      //                  catch
      //                  {
      //                  }
      //              }
      //              Recall recall = new Recall(registry, dataReaderWrapper1.GetInt32("RecordNumber"), dataReaderWrapper1.GetString("CampaignNumber"), recallDate, dataReaderWrapper1.GetString("DefectDescription"), dataReaderWrapper1.GetString("DefectDescription_es"), dataReaderWrapper1.GetString("DefectDescription_fr"), dataReaderWrapper1.GetString("DefectDescription_zh"), dataReaderWrapper1.GetString("DefectConsequence"), dataReaderWrapper1.GetString("DefectConsequence_es"), dataReaderWrapper1.GetString("DefectConsequence_fr"), dataReaderWrapper1.GetString("DefectConsequence_zh"), dataReaderWrapper1.GetString("DefectCorrectiveAction"), dataReaderWrapper1.GetString("DefectCorrectiveAction_es"), dataReaderWrapper1.GetString("DefectCorrectiveAction_fr"), dataReaderWrapper1.GetString("DefectCorrectiveAction_zh"));
      //              recallCollection.Add(recall);
      //          }
      //      }
      //      recallCollection.Sort(new SortDictionaryCollection()
      //{
      //  new SortDictionary("RecallDate", SortDirection.Descending)
      //});
      //      return recallCollection;
      //  }

      //  public static RecallCollection Search(Registry registry, int year, string make, string model, string trimLevel)
      //  {
      //      RecallCollection recallCollection = new RecallCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_Search";
      //          dataReaderWrapper.AddInt32("Year", year);
      //          dataReaderWrapper.AddNVarChar("Make", make);
      //          dataReaderWrapper.AddNVarChar("Model", model);
      //          dataReaderWrapper.AddNVarChar("Trim", trimLevel);
      //          dataReaderWrapper.Execute();
      //          while (dataReaderWrapper.Read())
      //          {
      //              DateTime recallDate = DateTime.MinValue;
      //              if (dataReaderWrapper.GetString("RecallDate") != "")
      //              {
      //                  try
      //                  {
      //                      recallDate = DateTime.ParseExact(dataReaderWrapper.GetString("RecallDate"), "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
      //                  }
      //                  catch
      //                  {
      //                  }
      //              }
      //              Recall recall = new Recall(registry, dataReaderWrapper.GetInt32("RecordNumber"), dataReaderWrapper.GetString("CampaignNumber"), recallDate, dataReaderWrapper.GetString("DefectDescription"), dataReaderWrapper.GetString("DefectDescription_es"), dataReaderWrapper.GetString("DefectDescription_fr"), dataReaderWrapper.GetString("DefectDescription_zh"), dataReaderWrapper.GetString("DefectConsequence"), dataReaderWrapper.GetString("DefectConsequence_es"), dataReaderWrapper.GetString("DefectConsequence_fr"), dataReaderWrapper.GetString("DefectConsequence_zh"), dataReaderWrapper.GetString("DefectCorrectiveAction"), dataReaderWrapper.GetString("DefectCorrectiveAction_es"), dataReaderWrapper.GetString("DefectCorrectiveAction_fr"), dataReaderWrapper.GetString("DefectCorrectiveAction_zh"));
      //              recallCollection.Add(recall);
      //          }
      //      }
      //      return recallCollection;
      //  }

      //  public static RecallCollection SearchWithPageSpanning(Registry registry, int currentPage, int pageSize, int year, string make, string model, string trimLevel, out Pagination pagination)
      //  {
      //      RecallCollection recallCollection = new RecallCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_SearchWithPageSpanning";
      //          dataReaderWrapper.AddInt32("CurrentPage", currentPage);
      //          dataReaderWrapper.AddInt32("PageSize", pageSize);
      //          dataReaderWrapper.AddInt32("Year", year);
      //          dataReaderWrapper.AddNVarChar("Make", make);
      //          dataReaderWrapper.AddNVarChar("Model", model);
      //          dataReaderWrapper.AddNVarChar("Trim", trimLevel);
      //          dataReaderWrapper.Execute();
      //          dataReaderWrapper.Read();
      //          pagination = new Pagination(dataReaderWrapper.GetInt32("AbsolutePage"), dataReaderWrapper.GetInt32("PageSize"), dataReaderWrapper.GetInt32("RecordCount"), dataReaderWrapper.GetInt32("MaxRecords"));
      //          dataReaderWrapper.NextResult();
      //          while (dataReaderWrapper.Read())
      //          {
      //              DateTime recallDate = DateTime.MinValue;
      //              if (dataReaderWrapper.GetString("RecallDate") != "")
      //              {
      //                  try
      //                  {
      //                      recallDate = DateTime.ParseExact(dataReaderWrapper.GetString("RecallDate"), "yyyymmdd", (IFormatProvider)CultureInfo.CurrentCulture.DateTimeFormat);
      //                  }
      //                  catch
      //                  {
      //                  }
      //              }
      //              Recall recall = new Recall(registry, dataReaderWrapper.GetInt32("RecordNumber"), dataReaderWrapper.GetString("CampaignNumber"), recallDate, dataReaderWrapper.GetString("DefectDescription"), dataReaderWrapper.GetString("DefectDescription_es"), dataReaderWrapper.GetString("DefectDescription_fr"), dataReaderWrapper.GetString("DefectDescription_zh"), dataReaderWrapper.GetString("DefectConsequence"), dataReaderWrapper.GetString("DefectConsequence_es"), dataReaderWrapper.GetString("DefectConsequence_fr"), dataReaderWrapper.GetString("DefectConsequence_zh"), dataReaderWrapper.GetString("DefectCorrectiveAction"), dataReaderWrapper.GetString("DefectCorrectiveAction_es"), dataReaderWrapper.GetString("DefectCorrectiveAction_fr"), dataReaderWrapper.GetString("DefectCorrectiveAction_zh"));
      //              recallCollection.Add(recall);
      //          }
      //      }
      //      return recallCollection;
      //  }

      //  public static ArrayList GetYears(string connectionString)
      //  {
      //      return Recall.GetYears(connectionString, SortDirection.Descending);
      //  }

      //  public static ArrayList GetYears(string connectionString, SortDirection sortDirection)
      //  {
      //      ArrayList arrayList = new ArrayList();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(connectionString))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_GetYears";
      //          dataReaderWrapper.AddInt32("SortDirection", (int)sortDirection);
      //          dataReaderWrapper.Execute();
      //          while (dataReaderWrapper.Read())
      //          {
      //              int int32 = dataReaderWrapper.GetInt32("Year");
      //              if (int32 > 1900 && int32 < 2100)
      //                  arrayList.Add((object)dataReaderWrapper.GetInt32("Year"));
      //          }
      //      }
      //      return arrayList;
      //  }

      //  public static StringCollection GetMakes(string connectionString, int year)
      //  {
      //      StringCollection stringCollection = new StringCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(connectionString))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_GetMakes";
      //          dataReaderWrapper.AddInt32("Year", year);
      //          dataReaderWrapper.Execute();
      //          while (dataReaderWrapper.Read())
      //          {
      //              string titleCase = dataReaderWrapper.GetString("Make");
      //              if (titleCase.ToUpper() != "GMC" && titleCase.ToUpper() != "BMW")
      //                  titleCase = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(titleCase.ToLower());
      //              stringCollection.Add(titleCase);
      //          }
      //      }
      //      return stringCollection;
      //  }

      //  public static StringCollection GetModels(string connectionString, int year, string make)
      //  {
      //      StringCollection stringCollection = new StringCollection();
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(connectionString))
      //      {
      //          dataReaderWrapper.ProcedureName = "Recall_GetModels";
      //          dataReaderWrapper.AddInt32("Year", year);
      //          dataReaderWrapper.AddNVarChar("Make", make);
      //          dataReaderWrapper.Execute();
      //          while (dataReaderWrapper.Read())
      //              stringCollection.Add(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dataReaderWrapper.GetString("Model").ToLower()));
      //      }
      //      return stringCollection;
      //  }

      //  public static void GetRecallDataUpdatedDate(Registry registry, out DateTime recallUpdatedDate, out DateTime recallUpdatedDatePrevious)
      //  {
      //      using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
      //      {
      //          dataReaderWrapper.ProcedureName = "_RecallTSBDataUpdatedDate";
      //          dataReaderWrapper.Execute();
      //          if (dataReaderWrapper.Read())
      //          {
      //              recallUpdatedDate = dataReaderWrapper.GetDateTime("RecallUpdated");
      //              recallUpdatedDatePrevious = dataReaderWrapper.GetDateTime("RecallUpdatedPrevious");
      //              return;
      //          }
      //      }
      //      throw new ApplicationException("Unable to find the procedure _RecallTSBDataUpdatedDate");
      //  }
    }
}
