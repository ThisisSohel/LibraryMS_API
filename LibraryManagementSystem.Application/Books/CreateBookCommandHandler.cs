using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Books;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<CreateBookCommandHandler> _logger;

    public CreateBookCommandHandler(IBookRepository bookRepository, ILogger<CreateBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (await _bookRepository.IsbnExistsAsync(request.Isbn, excludeId: null, cancellationToken))
        {
            throw new ConflictException($"A book with ISBN '{request.Isbn}' already exists.");
        }

        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            Category = request.Category,
            Publisher = request.Publisher,
            PublishedYear = request.PublishedYear
        };

        await _bookRepository.AddAsync(book, cancellationToken);
        await _bookRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Book {BookId} ({Isbn}) created: {Title}", book.Id, book.Isbn, book.Title);

        return BookDto.FromEntity(book);
    }
}
