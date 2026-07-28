using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Reservations;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly ILogger<CreateReservationCommandHandler> _logger;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IMemberRepository memberRepository,
        IBookRepository bookRepository,
        IBranchRepository branchRepository,
        IBookCopyRepository bookCopyRepository,
        ILogger<CreateReservationCommandHandler> logger)
    {
        _reservationRepository = reservationRepository;
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _branchRepository = branchRepository;
        _bookCopyRepository = bookCopyRepository;
        _logger = logger;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        if (!member.IsActive)
        {
            throw new ConflictException("Cannot create a reservation for an inactive member.");
        }

        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.BookId);

        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        var bookCopy = await _bookCopyRepository.GetByBookAndBranchAsync(request.BookId, request.BranchId, cancellationToken);

        if (bookCopy is not null && bookCopy.AvailableCopies > 0)
        {
            throw new ConflictException("Copies of this book are currently available at this branch — borrow it directly instead of reserving.");
        }

        if (await _reservationRepository.HasActiveReservationAsync(request.MemberId, request.BookId, request.BranchId, cancellationToken))
        {
            throw new ConflictException("This member already has an active reservation for this book at this branch.");
        }

        var queuePosition = await _reservationRepository.GetNextQueuePositionAsync(request.BookId, request.BranchId, cancellationToken);

        var reservation = new Reservation
        {
            MemberId = request.MemberId,
            Member = member,
            BookId = request.BookId,
            Book = book,
            BranchId = request.BranchId,
            Branch = branch,
            ReservationDate = DateTime.UtcNow,
            Status = ReservationStatus.Pending,
            QueuePosition = queuePosition
        };

        await _reservationRepository.AddAsync(reservation, cancellationToken);
        await _reservationRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Member {MemberId} reserved book {BookId} at branch {BranchId}, queue position {QueuePosition}",
            request.MemberId, request.BookId, request.BranchId, queuePosition);

        return ReservationDto.FromEntity(reservation);
    }
}
