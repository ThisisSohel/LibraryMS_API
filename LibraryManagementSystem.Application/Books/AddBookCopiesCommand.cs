using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record AddBookCopiesCommand(int BookId, int BranchId, int Quantity) : IRequest<BookCopyDto>;
