using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Books;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(IBookRepository bookRepository, ILogger<UpdateBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.Id);

        if (await _bookRepository.IsbnExistsAsync(request.Isbn, excludeId: request.Id, cancellationToken))
        {
            throw new ConflictException($"A book with ISBN '{request.Isbn}' already exists.");
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.Isbn = request.Isbn;
        book.Category = request.Category;
        book.Publisher = request.Publisher;
        book.PublishedYear = request.PublishedYear;

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Book {BookId} ({Isbn}) updated: {Title}", book.Id, book.Isbn, book.Title);

        return BookDto.FromEntity(book);
    }
}
