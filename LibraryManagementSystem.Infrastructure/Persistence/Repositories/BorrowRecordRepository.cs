using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<BorrowRecord> WithDetails() =>
        _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.ProcessedByUser)
            .Include(br => br.BookCopy).ThenInclude(bc => bc.Book)
            .Include(br => br.BookCopy).ThenInclude(bc => bc.Branch);

    public Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        WithDetails().FirstOrDefaultAsync(br => br.Id == id, cancellationToken);

    public async Task<(List<BorrowRecord> Items, int TotalCount)> GetAllAsync(
        int? memberId, int? branchId, BorrowStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = WithDetails();

        if (memberId.HasValue)
        {
            query = query.Where(br => br.MemberId == memberId.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(br => br.BookCopy.BranchId == branchId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(br => br.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(br => br.BorrowDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> HasActiveBorrowAsync(int memberId, int bookId, CancellationToken cancellationToken) =>
        _context.BorrowRecords.AnyAsync(br =>
            br.MemberId == memberId &&
            br.BookCopy.BookId == bookId &&
            br.Status == BorrowStatus.Borrowed,
            cancellationToken);

    public async Task AddAsync(BorrowRecord borrowRecord, CancellationToken cancellationToken) =>
        await _context.BorrowRecords.AddAsync(borrowRecord, cancellationToken);

    public void Update(BorrowRecord borrowRecord) => _context.BorrowRecords.Update(borrowRecord);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
