using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record ExportBranchInventorySummaryReportQuery(ReportExportFormat Format) : IRequest<ReportFileDto>;
