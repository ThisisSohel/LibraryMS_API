using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public record GetAllReservationsQuery(
    int? MemberId,
    int? BookId,
    int? BranchId,
    string? Status,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<ReservationDto>>;
