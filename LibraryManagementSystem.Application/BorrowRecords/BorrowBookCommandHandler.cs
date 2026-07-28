using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowRecordDto>
{
    private const int LoanPeriodDays = 14;

    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<BorrowBookCommandHandler> _logger;

    public BorrowBookCommandHandler(
        IBorrowRecordRepository borrowRecordRepository,
        IMemberRepository memberRepository,
        IBookCopyRepository bookCopyRepository,
        IUserRepository userRepository,
        ILogger<BorrowBookCommandHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _memberRepository = memberRepository;
        _bookCopyRepository = bookCopyRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<BorrowRecordDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        if (!member.IsActive)
        {
            throw new ConflictException("Cannot process a borrow for an inactive member.");
        }

        var processedByUser = await _userRepository.GetByIdAsync(request.ProcessedByUserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.ProcessedByUserId);

        var bookCopy = await _bookCopyRepository.GetByBookAndBranchAsync(request.BookId, request.BranchId, cancellationToken)
            ?? throw new NotFoundException("BookCopy", $"bookId={request.BookId}, branchId={request.BranchId}");

        if (bookCopy.AvailableCopies <= 0)
        {
            throw new ConflictException("No copies of this book are currently available at this branch.");
        }

        if (await _borrowRecordRepository.HasActiveBorrowAsync(request.MemberId, request.BookId, cancellationToken))
        {
            throw new ConflictException("This member already has an active borrow of this book.");
        }

        bookCopy.AvailableCopies--;
        _bookCopyRepository.Update(bookCopy);

        var borrowRecord = new BorrowRecord
        {
            MemberId = request.MemberId,
            Member = member,
            BookCopyId = bookCopy.Id,
            BookCopy = bookCopy,
            ProcessedByUserId = request.ProcessedByUserId,
            ProcessedByUser = processedByUser,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(LoanPeriodDays),
            Status = BorrowStatus.Borrowed
        };

        await _borrowRecordRepository.AddAsync(borrowRecord, cancellationToken);
        await _borrowRecordRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Member {MemberId} borrowed book {BookId} (copy {BookCopyId}) at branch {BranchId}, due {DueDate}",
            request.MemberId, request.BookId, bookCopy.Id, request.BranchId, borrowRecord.DueDate);

        return BorrowRecordDto.FromEntity(borrowRecord);
    }
}
