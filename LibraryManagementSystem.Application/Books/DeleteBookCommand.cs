using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record DeleteBookCommand(int Id) : IRequest;
