using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public record CreateReservationCommand(int MemberId, int BookId, int BranchId) : IRequest<ReservationDto>;
