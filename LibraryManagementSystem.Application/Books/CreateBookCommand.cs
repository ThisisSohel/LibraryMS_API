using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record CreateBookCommand(
    string Title,
    string Author,
    string Isbn,
    string? Category,
    string? Publisher,
    int? PublishedYear) : IRequest<BookDto>;
