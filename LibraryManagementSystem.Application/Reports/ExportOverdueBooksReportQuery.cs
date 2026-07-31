using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record ExportOverdueBooksReportQuery(int? BranchId, ReportExportFormat Format) : IRequest<ReportFileDto>;
