using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record GetBookCopiesQuery(int BookId) : IRequest<List<BookCopyDto>>;
