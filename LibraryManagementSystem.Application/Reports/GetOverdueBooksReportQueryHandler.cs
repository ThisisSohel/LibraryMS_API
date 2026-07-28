using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class GetOverdueBooksReportQueryHandler : IRequestHandler<GetOverdueBooksReportQuery, List<OverdueBookDto>>
{
    private readonly IReportsRepository _reportsRepository;

    public GetOverdueBooksReportQueryHandler(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public Task<List<OverdueBookDto>> Handle(GetOverdueBooksReportQuery request, CancellationToken cancellationToken) =>
        _reportsRepository.GetOverdueBooksAsync(request.BranchId, cancellationToken);
}
