using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public class GetBranchInventorySummaryReportQueryHandler : IRequestHandler<GetBranchInventorySummaryReportQuery, List<BranchInventorySummaryDto>>
{
    private readonly IReportsRepository _reportsRepository;

    public GetBranchInventorySummaryReportQueryHandler(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public Task<List<BranchInventorySummaryDto>> Handle(GetBranchInventorySummaryReportQuery request, CancellationToken cancellationToken) =>
        _reportsRepository.GetBranchInventorySummaryAsync(cancellationToken);
}
