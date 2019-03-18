namespace DataAccessLayers.WebService
{
    public  class DiagnosticReportLogging : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback LogDiagnosticReportOperationCompleted;

        private System.Threading.SendOrPostCallback LogDiagnosticReportWithMileageOperationCompleted;

        private System.Threading.SendOrPostCallback GetTSBCountByVehicleOperationCompleted;

        private System.Threading.SendOrPostCallback GetRecallsCountForVehicleOperationCompleted;

        private System.Threading.SendOrPostCallback GetDTCLibraryErrorCodeWithLaymensTermsOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;
    }
}
