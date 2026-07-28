using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Reservation), request.Id);

        return ReservationDto.FromEntity(reservation);
    }
}
