namespace LibraryManagementSystem.Application.Reports;

public enum ReportExportFormat
{
    Xlsx,
    Pdf
}

public static class ReportExportFormatExtensions
{
    public static string GetContentType(this ReportExportFormat format) => format switch
    {
        ReportExportFormat.Xlsx => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ReportExportFormat.Pdf => "application/pdf",
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };

    public static string GetFileExtension(this ReportExportFormat format) => format switch
    {
        ReportExportFormat.Xlsx => "xlsx",
        ReportExportFormat.Pdf => "pdf",
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };
}
