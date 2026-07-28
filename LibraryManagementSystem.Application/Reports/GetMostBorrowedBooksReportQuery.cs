using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record GetMostBorrowedBooksReportQuery(int? BranchId, int Top = 10) : IRequest<List<MostBorrowedBookDto>>;
