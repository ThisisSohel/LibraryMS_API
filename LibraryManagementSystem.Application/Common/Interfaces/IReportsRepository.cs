using LibraryManagementSystem.Application.Reports;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IReportsRepository
{
    Task<List<OverdueBookDto>> GetOverdueBooksAsync(int? branchId, CancellationToken cancellationToken);
    Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int? branchId, int top, CancellationToken cancellationToken);
    Task<List<BranchInventorySummaryDto>> GetBranchInventorySummaryAsync(CancellationToken cancellationToken);
}
