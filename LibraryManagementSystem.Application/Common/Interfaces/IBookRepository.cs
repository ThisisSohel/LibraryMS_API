using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Book?> GetByIdWithCopiesAsync(int id, CancellationToken cancellationToken);
    Task<(List<Book> Items, int TotalCount)> GetAllAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> IsbnExistsAsync(string isbn, int? excludeId, CancellationToken cancellationToken);
    Task AddAsync(Book book, CancellationToken cancellationToken);
    void Update(Book book);
    void Delete(Book book);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
