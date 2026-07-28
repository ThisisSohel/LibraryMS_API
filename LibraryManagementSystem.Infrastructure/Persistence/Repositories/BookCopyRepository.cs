using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class BookCopyRepository : IBookCopyRepository
{
    private readonly AppDbContext _context;

    public BookCopyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<BookCopy?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.BookCopies
            .Include(bc => bc.Branch)
            .Include(bc => bc.Book)
            .FirstOrDefaultAsync(bc => bc.Id == id, cancellationToken);

    public Task<BookCopy?> GetByBookAndBranchAsync(int bookId, int branchId, CancellationToken cancellationToken) =>
        _context.BookCopies
            .Include(bc => bc.Branch)
            .Include(bc => bc.Book)
            .FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.BranchId == branchId, cancellationToken);

    public async Task<List<BookCopy>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken) =>
        await _context.BookCopies
            .Include(bc => bc.Branch)
            .Where(bc => bc.BookId == bookId)
            .OrderBy(bc => bc.Branch.Name)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(BookCopy bookCopy, CancellationToken cancellationToken) =>
        await _context.BookCopies.AddAsync(bookCopy, cancellationToken);

    public void Update(BookCopy bookCopy) => _context.BookCopies.Update(bookCopy);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
