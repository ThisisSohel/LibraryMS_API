using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class ExportMostBorrowedBooksReportQueryHandler : IRequestHandler<ExportMostBorrowedBooksReportQuery, ReportFileDto>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly IReportExportService _reportExportService;

    public ExportMostBorrowedBooksReportQueryHandler(
        IReportsRepository reportsRepository,
        IReportExportService reportExportService)
    {
        _reportsRepository = reportsRepository;
        _reportExportService = reportExportService;
    }

    public async Task<ReportFileDto> Handle(ExportMostBorrowedBooksReportQuery request, CancellationToken cancellationToken)
    {
        var items = await _reportsRepository.GetMostBorrowedBooksAsync(request.BranchId, request.Top, cancellationToken);
        var content = _reportExportService.ExportMostBorrowedBooks(items, request.Format);

        return new ReportFileDto(
            content,
            request.Format.GetContentType(),
            $"most-borrowed-books.{request.Format.GetFileExtension()}");
    }
}
