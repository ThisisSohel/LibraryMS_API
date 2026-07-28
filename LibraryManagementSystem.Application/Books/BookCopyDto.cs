using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Books;

public record BookCopyDto(
    int Id,
    int BookId,
    int BranchId,
    string BranchName,
    int TotalCopies,
    int AvailableCopies)
{
    public static BookCopyDto FromEntity(BookCopy bookCopy) => new(
        bookCopy.Id,
        bookCopy.BookId,
        bookCopy.BranchId,
        bookCopy.Branch.Name,
        bookCopy.TotalCopies,
        bookCopy.AvailableCopies);
}
