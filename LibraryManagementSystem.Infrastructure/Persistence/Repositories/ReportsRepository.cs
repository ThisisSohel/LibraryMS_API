using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Reports;
using LibraryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly AppDbContext _context;

    public ReportsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OverdueBookDto>> GetOverdueBooksAsync(int? branchId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var query = _context.BorrowRecords
            .Where(br => br.Status == BorrowStatus.Borrowed && br.DueDate < now);

        if (branchId.HasValue)
        {
            query = query.Where(br => br.BookCopy.BranchId == branchId.Value);
        }

        // Project the raw columns first (translates cleanly to SQL), then compute
        // DaysOverdue in memory to avoid relying on TimeSpan/TotalDays SQL translation.
        var raw = await query
            .OrderBy(br => br.DueDate)
            .Select(br => new
            {
                br.Id,
                br.MemberId,
                MemberName = br.Member.FullName,
                MemberEmail = br.Member.Email,
                BookId = br.BookCopy.BookId,
                BookTitle = br.BookCopy.Book.Title,
                BranchId = br.BookCopy.BranchId,
                BranchName = br.BookCopy.Branch.Name,
                br.BorrowDate,
                br.DueDate
            })
            .ToListAsync(cancellationToken);

        return raw
            .Select(r => new OverdueBookDto(
                r.Id, r.MemberId, r.MemberName, r.MemberEmail,
                r.BookId, r.BookTitle, r.BranchId, r.BranchName,
                r.BorrowDate, r.DueDate, (int)(now - r.DueDate).TotalDays))
            .ToList();
    }

    public async Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int? branchId, int top, CancellationToken cancellationToken)
    {
        var query = _context.BorrowRecords.AsQueryable();

        if (branchId.HasValue)
        {
            query = query.Where(br => br.BookCopy.BranchId == branchId.Value);
        }

        // Group by BookId alone (a single join to BookCopy) rather than including Book's
        // Title/Author in the group key — grouping across a second join didn't translate to SQL.
        var topBookCounts = await query
            .GroupBy(br => br.BookCopy.BookId)
            .Select(g => new { BookId = g.Key, BorrowCount = g.Count() })
            .OrderByDescending(x => x.BorrowCount)
            .Take(top)
            .ToListAsync(cancellationToken);

        var bookIds = topBookCounts.Select(x => x.BookId).ToList();

        var books = await _context.Books
            .Where(b => bookIds.Contains(b.Id))
            .Select(b => new { b.Id, b.Title, b.Author })
            .ToListAsync(cancellationToken);

        return topBookCounts
            .Join(books, tc => tc.BookId, b => b.Id, (tc, b) => new MostBorrowedBookDto(b.Id, b.Title, b.Author, tc.BorrowCount))
            .ToList();
    }

    public async Task<List<BranchInventorySummaryDto>> GetBranchInventorySummaryAsync(CancellationToken cancellationToken) =>
        await _context.Branches
            .OrderBy(b => b.Name)
            .Select(b => new BranchInventorySummaryDto(
                b.Id,
                b.Name,
                b.BookCopies.Select(bc => bc.BookId).Distinct().Count(),
                b.BookCopies.Sum(bc => (int?)bc.TotalCopies) ?? 0,
                b.BookCopies.Sum(bc => (int?)bc.AvailableCopies) ?? 0))
            .ToListAsync(cancellationToken);
}
