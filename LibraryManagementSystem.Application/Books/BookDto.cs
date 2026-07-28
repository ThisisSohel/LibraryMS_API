using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Books;

public record BookDto(
    int Id,
    string Title,
    string Author,
    string Isbn,
    string? Category,
    string? Publisher,
    int? PublishedYear,
    DateTime CreatedAt)
{
    public static BookDto FromEntity(Book book) => new(
        book.Id,
        book.Title,
        book.Author,
        book.Isbn,
        book.Category,
        book.Publisher,
        book.PublishedYear,
        book.CreatedAt);
}
