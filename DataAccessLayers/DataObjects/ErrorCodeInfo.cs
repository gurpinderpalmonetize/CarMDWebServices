using DataAccessLayers.DataBase;

namespace DataAccessLayers.DataObjects
{
    public class ErrorCodeInfo
    {
        public ErrorCodeInfo()
        {
        }
        public string Code { get; set; }
        public int CodeType { get; set; }
        public int ErrorCodeSystemType { get; set; }
        public ErrorCodeInfoDefinition[] ErrorCodeDefinitions { get; set; }
        public bool HasMultipleDefinitions { get; set; }

        protected internal static ErrorCodeInfo GetWebServiceObject(DiagnosticReportResultErrorCode sdkObject)
        {
            return GetWebServiceObject(sdkObject, true);
        }

      
        protected internal static ErrorCodeInfo GetWebServiceObject(DiagnosticReportResultErrorCode sdkObject, bool includeLaymansTerms)
        {
            ErrorCodeInfo wsObject = new ErrorCodeInfo();

            //set the error code string
            wsObject.Code = sdkObject.ErrorCode;
            //set the error code type
            wsObject.CodeType = (int)sdkObject.DiagnosticReportErrorCodeType;
            //set the error code system type
            wsObject.ErrorCodeSystemType = (int)sdkObject.DiagnosticReportErrorCodeSystemType;


            //now loop through the definitions
            //DiagnosticReportResultErrorCodeDefinitionDisplayCollection defs = sdkObject.GetDiagnosticReportResultErrorCodeDefinitions(DiagnosticReportResultType.CarScan);

            ////set the local variable equal to 1 if the defininitions have 
            //wsObject.ErrorCodeDefinitions = new ErrorCodeInfoDefinition[defs.Count];

            ////determine whether or not the object has multiple definitions
            //wsObject.HasMultipleDefinitions = defs.Count > 1;

            //for (int i = 0; i < defs.Count; i++)
            //{
            //    //get the web service object here, pass in to the next method whether or not there are multiple definitions
            //    wsObject.ErrorCodeDefinitions[i] = ErrorCodeInfoDefinition.GetWebServiceObject(defs[i], wsObject.HasMultipleDefinitions, includeLaymansTerms);

            //}

            return wsObject;

        }
    }
}
