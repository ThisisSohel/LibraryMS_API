using LibraryManagementSystem.Application.BorrowRecords;
using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Reservations;

public class FulfillReservationCommandHandler : IRequestHandler<FulfillReservationCommand, BorrowRecordDto>
{
    private const int LoanPeriodDays = 14;

    private readonly IReservationRepository _reservationRepository;
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<FulfillReservationCommandHandler> _logger;

    public FulfillReservationCommandHandler(
        IReservationRepository reservationRepository,
        IBookCopyRepository bookCopyRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IUserRepository userRepository,
        ILogger<FulfillReservationCommandHandler> logger)
    {
        _reservationRepository = reservationRepository;
        _bookCopyRepository = bookCopyRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<BorrowRecordDto> Handle(FulfillReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Reservation), request.Id);

        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new ConflictException($"Only pending reservations can be fulfilled. This reservation is '{reservation.Status}'.");
        }

        var frontOfQueue = await _reservationRepository.GetFrontOfQueueAsync(reservation.BookId, reservation.BranchId, cancellationToken);

        if (frontOfQueue is not null && frontOfQueue.Id != reservation.Id)
        {
            throw new ConflictException($"Cannot fulfill out of turn — reservation {frontOfQueue.Id} is ahead in the queue.");
        }

        var processedByUser = await _userRepository.GetByIdAsync(request.ProcessedByUserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.ProcessedByUserId);

        var bookCopy = await _bookCopyRepository.GetByBookAndBranchAsync(reservation.BookId, reservation.BranchId, cancellationToken);

        if (bookCopy is null || bookCopy.AvailableCopies <= 0)
        {
            throw new ConflictException("No copies are currently available to fulfill this reservation.");
        }

        bookCopy.AvailableCopies--;
        _bookCopyRepository.Update(bookCopy);

        var borrowRecord = new BorrowRecord
        {
            MemberId = reservation.MemberId,
            Member = reservation.Member,
            BookCopyId = bookCopy.Id,
            BookCopy = bookCopy,
            ProcessedByUserId = request.ProcessedByUserId,
            ProcessedByUser = processedByUser,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(LoanPeriodDays),
            Status = BorrowStatus.Borrowed
        };

        await _borrowRecordRepository.AddAsync(borrowRecord, cancellationToken);

        reservation.Status = ReservationStatus.Fulfilled;
        _reservationRepository.Update(reservation);

        var behind = await _reservationRepository.GetQueueBehindAsync(
            reservation.BookId, reservation.BranchId, reservation.QueuePosition, cancellationToken);

        foreach (var r in behind)
        {
            r.QueuePosition--;
            _reservationRepository.Update(r);
        }

        await _reservationRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Reservation {ReservationId} fulfilled: member {MemberId} borrowed book {BookId} (copy {BookCopyId})",
            reservation.Id, reservation.MemberId, reservation.BookId, bookCopy.Id);

        return BorrowRecordDto.FromEntity(borrowRecord);
    }
}
