using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public Task<Book?> GetByIdWithCopiesAsync(int id, CancellationToken cancellationToken) =>
        _context.Books
            .Include(b => b.BookCopies)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<(List<Book> Items, int TotalCount)> GetAllAsync(
        string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                EF.Functions.ILike(b.Title, $"%{search}%") ||
                EF.Functions.ILike(b.Author, $"%{search}%") ||
                EF.Functions.ILike(b.Isbn, $"%{search}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> IsbnExistsAsync(string isbn, int? excludeId, CancellationToken cancellationToken)
    {
        var query = _context.Books.Where(b => b.Isbn == isbn);

        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Id != excludeId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken) =>
        await _context.Books.AddAsync(book, cancellationToken);

    public void Update(Book book) => _context.Books.Update(book);

    public void Delete(Book book) => _context.Books.Remove(book);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
