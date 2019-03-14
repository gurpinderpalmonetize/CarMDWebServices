using DataAccessLayers.DataBase;

namespace DataAccessLayers.DataObjects
{
    public class TSBInfo
    {
        public TSBInfo()
        {
        }
        public int TsbId { get; set; }
        public string Description { get; set; }
        public string FileNamePDF { get; set; }
        public string PDFFileUrl { get; set; }
        public string ManufacturerNumber { get; set; }
        public string IssueDateString { get; set; }
        public string TsbText { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string AutoSystem { get; set; }
        public string[] DTCcodes { get; set; }
        public TSBTypeInfo[] TSBTypes { get; set; }
        public TSBCategoryInfo[] TSBCategories { get; set; }
        protected internal static TSBInfo GetWebServiceObject(TSB sdkObject)
        {
            TSBInfo wsObject = new TSBInfo();
            //wsObject.TsbId = sdkObject.TsbId;
            //wsObject.Description = sdkObject.Description;


            //wsObject.FileNamePDF = sdkObject.FileNamePDF;
            //wsObject.PDFFileUrl = sdkObject.GetTsbPdfUrl(Global.TsbRootUrl);
            //wsObject.ManufacturerNumber = sdkObject.ManufacturerNumber;

            //wsObject.IssueDateString = sdkObject.IssueDate.ToShortDateString();
            //wsObject.TsbText = sdkObject.TsbText;

            //wsObject.CreatedDateString = sdkObject.CreatedDateTime.ToShortDateString();
            //wsObject.UpdatedDateString = sdkObject.UpdatedDateTime.ToShortDateString();

            //wsObject.System = sdkObject.System;
            //wsObject.SubSystem = sdkObject.SubSystem;
            //wsObject.AutoSystem = sdkObject.AutoSystem;

            //if (sdkObject.DTCcodes != null && sdkObject.DTCcodes.Count > 0)
            //{
            //    wsObject.DTCcodes = new string[sdkObject.DTCcodes.Count];

            //    for (int i = 0; i < sdkObject.DTCcodes.Count; i++)
            //    {
            //        wsObject.DTCcodes[i] = sdkObject.DTCcodes[i];
            //    }

            //}
            //if (sdkObject.TsbTypes != null && sdkObject.TsbTypes.Count > 0)
            //{
            //    wsObject.TSBTypes = new TSBTypeInfo[sdkObject.TsbTypes.Count];

            //    for (int i = 0; i < sdkObject.TsbTypes.Count; i++)
            //    {
            //        wsObject.TSBTypes[i] = TSBTypeInfo.GetWebServiceObject(sdkObject.TsbTypes[i]);
            //    }

            //}


            //if (sdkObject.TsbCategories != null && sdkObject.TsbCategories.Count > 0)
            //{
            //    wsObject.TSBCategories = new TSBCategoryInfo[sdkObject.TsbCategories.Count];

            //    for (int i = 0; i < sdkObject.TsbCategories.Count; i++)
            //    {
            //        wsObject.TSBCategories[i] = TSBCategoryInfo.GetWebServiceObject(sdkObject.TsbCategories[i]);
            //    }

            //}

            return wsObject;

        }

    }
}
