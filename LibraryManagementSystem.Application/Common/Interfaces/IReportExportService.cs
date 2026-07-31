using LibraryManagementSystem.Application.Reports;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IReportExportService
{
    byte[] ExportOverdueBooks(IReadOnlyList<OverdueBookDto> items, ReportExportFormat format);
    byte[] ExportMostBorrowedBooks(IReadOnlyList<MostBorrowedBookDto> items, ReportExportFormat format);
    byte[] ExportBranchInventorySummary(IReadOnlyList<BranchInventorySummaryDto> items, ReportExportFormat format);
}
