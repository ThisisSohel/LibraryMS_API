using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class GetMostBorrowedBooksReportQueryHandler : IRequestHandler<GetMostBorrowedBooksReportQuery, List<MostBorrowedBookDto>>
{
    private readonly IReportsRepository _reportsRepository;

    public GetMostBorrowedBooksReportQueryHandler(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public Task<List<MostBorrowedBookDto>> Handle(GetMostBorrowedBooksReportQuery request, CancellationToken cancellationToken) =>
        _reportsRepository.GetMostBorrowedBooksAsync(request.BranchId, request.Top, cancellationToken);
}
