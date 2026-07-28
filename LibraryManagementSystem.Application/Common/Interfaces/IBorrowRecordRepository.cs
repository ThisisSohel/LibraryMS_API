using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(List<BorrowRecord> Items, int TotalCount)> GetAllAsync(
        int? memberId, int? branchId, BorrowStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> HasActiveBorrowAsync(int memberId, int bookId, CancellationToken cancellationToken);
    Task AddAsync(BorrowRecord borrowRecord, CancellationToken cancellationToken);
    void Update(BorrowRecord borrowRecord);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
