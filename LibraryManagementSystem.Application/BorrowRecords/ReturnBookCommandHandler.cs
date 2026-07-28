using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly ILogger<ReturnBookCommandHandler> _logger;

    public ReturnBookCommandHandler(
        IBorrowRecordRepository borrowRecordRepository,
        IBookCopyRepository bookCopyRepository,
        ILogger<ReturnBookCommandHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _bookCopyRepository = bookCopyRepository;
        _logger = logger;
    }

    public async Task<BorrowRecordDto> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(request.BorrowRecordId, cancellationToken)
            ?? throw new NotFoundException(nameof(BorrowRecord), request.BorrowRecordId);

        if (borrowRecord.Status == BorrowStatus.Returned)
        {
            throw new ConflictException("This borrow record has already been returned.");
        }

        borrowRecord.Status = BorrowStatus.Returned;
        borrowRecord.ReturnDate = DateTime.UtcNow;
        _borrowRecordRepository.Update(borrowRecord);

        borrowRecord.BookCopy.AvailableCopies++;
        _bookCopyRepository.Update(borrowRecord.BookCopy);

        await _borrowRecordRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Borrow record {BorrowRecordId} returned (member {MemberId}, book {BookId})",
            borrowRecord.Id, borrowRecord.MemberId, borrowRecord.BookCopy.BookId);

        return BorrowRecordDto.FromEntity(borrowRecord);
    }
}
