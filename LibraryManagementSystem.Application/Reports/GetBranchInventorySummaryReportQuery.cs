using MediatR;

namespace LibraryManagementSystem.Application.Reports;

public record GetBranchInventorySummaryReportQuery : IRequest<List<BranchInventorySummaryDto>>;
