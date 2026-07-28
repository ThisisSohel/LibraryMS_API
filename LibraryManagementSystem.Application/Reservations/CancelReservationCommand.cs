using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public record CancelReservationCommand(int Id) : IRequest<ReservationDto>;
