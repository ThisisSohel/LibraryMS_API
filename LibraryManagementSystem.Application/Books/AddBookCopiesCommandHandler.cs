using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Books;

public class AddBookCopiesCommandHandler : IRequestHandler<AddBookCopiesCommand, BookCopyDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly ILogger<AddBookCopiesCommandHandler> _logger;

    public AddBookCopiesCommandHandler(
        IBookRepository bookRepository,
        IBranchRepository branchRepository,
        IBookCopyRepository bookCopyRepository,
        ILogger<AddBookCopiesCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _branchRepository = branchRepository;
        _bookCopyRepository = bookCopyRepository;
        _logger = logger;
    }

    public async Task<BookCopyDto> Handle(AddBookCopiesCommand request, CancellationToken cancellationToken)
    {
        _ = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.BookId);

        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        var bookCopy = await _bookCopyRepository.GetByBookAndBranchAsync(request.BookId, request.BranchId, cancellationToken);

        if (bookCopy is null)
        {
            bookCopy = new BookCopy
            {
                BookId = request.BookId,
                BranchId = request.BranchId,
                Branch = branch,
                TotalCopies = request.Quantity,
                AvailableCopies = request.Quantity
            };

            await _bookCopyRepository.AddAsync(bookCopy, cancellationToken);
        }
        else
        {
            bookCopy.TotalCopies += request.Quantity;
            bookCopy.AvailableCopies += request.Quantity;
            _bookCopyRepository.Update(bookCopy);
        }

        await _bookCopyRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Added {Quantity} copies of book {BookId} at branch {BranchId} (total now {Total})",
            request.Quantity, request.BookId, request.BranchId, bookCopy.TotalCopies);

        return BookCopyDto.FromEntity(bookCopy);
    }
}
