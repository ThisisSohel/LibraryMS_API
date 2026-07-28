using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Books;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<DeleteBookCommandHandler> _logger;

    public DeleteBookCommandHandler(IBookRepository bookRepository, ILogger<DeleteBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdWithCopiesAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.Id);

        if (book.BookCopies.Count > 0)
        {
            throw new ConflictException("Cannot delete a book that has copies registered at a branch. Remove its copies first.");
        }

        _bookRepository.Delete(book);
        await _bookRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Book {BookId} ({Isbn}) deleted: {Title}", book.Id, book.Isbn, book.Title);
    }
}
