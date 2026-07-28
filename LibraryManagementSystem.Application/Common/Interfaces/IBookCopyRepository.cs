using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IBookCopyRepository
{
    Task<BookCopy?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<BookCopy?> GetByBookAndBranchAsync(int bookId, int branchId, CancellationToken cancellationToken);
    Task<List<BookCopy>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken);
    Task AddAsync(BookCopy bookCopy, CancellationToken cancellationToken);
    void Update(BookCopy bookCopy);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
