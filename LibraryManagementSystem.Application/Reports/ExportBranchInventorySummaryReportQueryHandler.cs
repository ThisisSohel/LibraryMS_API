using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class ExportBranchInventorySummaryReportQueryHandler : IRequestHandler<ExportBranchInventorySummaryReportQuery, ReportFileDto>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly IReportExportService _reportExportService;

    public ExportBranchInventorySummaryReportQueryHandler(
        IReportsRepository reportsRepository,
        IReportExportService reportExportService)
    {
        _reportsRepository = reportsRepository;
        _reportExportService = reportExportService;
    }

    public async Task<ReportFileDto> Handle(ExportBranchInventorySummaryReportQuery request, CancellationToken cancellationToken)
    {
        var items = await _reportsRepository.GetBranchInventorySummaryAsync(cancellationToken);
        var content = _reportExportService.ExportBranchInventorySummary(items, request.Format);

        return new ReportFileDto(
            content,
            request.Format.GetContentType(),
            $"branch-inventory-summary.{request.Format.GetFileExtension()}");
    }
}
