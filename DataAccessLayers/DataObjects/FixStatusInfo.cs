namespace DataAccessLayers.DataObjects
{
    public class FixStatusInfo
    {
        public FixStatusInfo()
        {

        }
        public DiagReportInfo DiagnosticReportInfo;
        public int PwrFixStatus { get; set; }
        public string PwrFixStatusDesc { get; set; }
        public string PwrFixLookupCancelledReason { get; set; }
        public int Obd1FixStatus { get; set; }
        public string Obd1FixStatusDesc { get; set; }
        public string Obd1FixLookupCancelledReason { get; set; }
        public int AbsFixStatus { get; set; }
        public string AbsFixStatusDesc { get; set; }
        public string AbsFixLookupCancelledReason { get; set; }
        public int SrsFixStatus { get; set; }
        public string SrsFixStatusDesc { get; set; }
        public string SrsFixLookupCancelledReason { get; set; }
    }
}
