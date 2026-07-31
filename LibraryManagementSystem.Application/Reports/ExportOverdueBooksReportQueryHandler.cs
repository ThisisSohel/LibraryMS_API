using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class ExportOverdueBooksReportQueryHandler : IRequestHandler<ExportOverdueBooksReportQuery, ReportFileDto>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly IReportExportService _reportExportService;

    public ExportOverdueBooksReportQueryHandler(
        IReportsRepository reportsRepository,
        IReportExportService reportExportService)
    {
        _reportsRepository = reportsRepository;
        _reportExportService = reportExportService;
    }

    public async Task<ReportFileDto> Handle(ExportOverdueBooksReportQuery request, CancellationToken cancellationToken)
    {
        var items = await _reportsRepository.GetOverdueBooksAsync(request.BranchId, cancellationToken);
        var content = _reportExportService.ExportOverdueBooks(items, request.Format);

        return new ReportFileDto(
            content,
            request.Format.GetContentType(),
            $"overdue-books.{request.Format.GetFileExtension()}");
    }
}
