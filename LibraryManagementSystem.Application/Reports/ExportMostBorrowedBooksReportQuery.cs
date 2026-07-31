using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record ExportMostBorrowedBooksReportQuery(int? BranchId, int Top, ReportExportFormat Format) : IRequest<ReportFileDto>;
