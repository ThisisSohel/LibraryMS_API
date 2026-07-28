using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public record GetReservationByIdQuery(int Id) : IRequest<ReservationDto>;
