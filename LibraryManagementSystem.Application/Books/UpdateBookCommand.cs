using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record UpdateBookCommand(
    int Id,
    string Title,
    string Author,
    string Isbn,
    string? Category,
    string? Publisher,
    int? PublishedYear) : IRequest<BookDto>;
