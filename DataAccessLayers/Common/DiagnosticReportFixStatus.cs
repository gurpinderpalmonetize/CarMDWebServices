using System.ComponentModel;

namespace DataAccessLayers.Common
{
    public enum DiagnosticReportFixStatus
    {

        [Description("Fix Not Needed")]
        FixNotNeeded = 0,

        [Description("Fix Found")]
        FixFound = 1,

        [Description("Fix Not Found")]
        FixNotFound = 2,

        [Description("Fix Not Found - Lookup Canceled")]
        FixNotFoundLookupCanceled = 3
    }
    public enum FixDifficultyRating
    {
        [Description("Difficulty Rating 1")]
        DifficultyRating1 = 0,
        [Description("Difficulty Rating 2")]
        DifficultyRating2 = 1,
        [Description("Difficulty Rating 3")]
        DifficultyRating3 = 2,
        [Description("Difficulty Rating 4")]
        DifficultyRating4 = 3,
        [Description("Difficulty Rating 5")]
        DifficultyRating5 = 4
    }
    public enum Currency
    {
        [Description("US Dollars")] USD,
        [Description("Canadian Dollars")] CAD,
        [Description("Euros")] EUR,
        [Description("Chinese Yuan")] CNY,
        [Description("Mexico Nuevo Peso")] MXN,
    }
    public enum DiagnosticReportErrorCodeSystemType
    {
        [Description("OBD2")]
        PowertrainObd2 = 0,
        [Description("OBD1")]
        PowertrainOBD1 = 1,
        [Description("ABS")]
        ABS = 2,
        [Description("SRS")]
        SRS = 3,
        [Description("Enhanced")]
        Enhanced = 4
    }

    public enum DiagnosticReportErrorCodeType
    {
        PrimaryDiagnosticReportErrorCode,
        FirstStoredDiagnosticReportErrorCode,
        AdditionalStoredDiagnosticReportErrorCode,
        FirstPendingDiagnosticReportErrorCode,
        AdditionalPendingDiagnosticReportErrorCode,
        FirstPermanentDiagnosticReportErrorCode,
        AdditionalPermanentDiagnosticReportErrorCode
    }
}
