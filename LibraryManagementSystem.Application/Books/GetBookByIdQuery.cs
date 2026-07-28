using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record GetBookByIdQuery(int Id) : IRequest<BookDto>;
