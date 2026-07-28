using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Reservations;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ILogger<CancelReservationCommandHandler> _logger;

    public CancelReservationCommandHandler(IReservationRepository reservationRepository, ILogger<CancelReservationCommandHandler> logger)
    {
        _reservationRepository = reservationRepository;
        _logger = logger;
    }

    public async Task<ReservationDto> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Reservation), request.Id);

        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new ConflictException($"Only pending reservations can be cancelled. This reservation is '{reservation.Status}'.");
        }

        reservation.Status = ReservationStatus.Cancelled;
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
            "Reservation {ReservationId} cancelled (member {MemberId}, book {BookId})",
            reservation.Id, reservation.MemberId, reservation.BookId);

        return ReservationDto.FromEntity(reservation);
    }
}
