using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record GetOverdueBooksReportQuery(int? BranchId) : IRequest<List<OverdueBookDto>>;
