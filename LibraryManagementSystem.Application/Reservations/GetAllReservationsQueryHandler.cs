using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Models;
using LibraryManagementSystem.Domain.Enums;
using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, PagedResult<ReservationDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<PagedResult<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        ReservationStatus? status = string.IsNullOrWhiteSpace(request.Status)
            ? null
            : Enum.Parse<ReservationStatus>(request.Status, ignoreCase: true);

        var (items, totalCount) = await _reservationRepository.GetAllAsync(
            request.MemberId, request.BookId, request.BranchId, status, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = items.Select(ReservationDto.FromEntity).ToList();

        return new PagedResult<ReservationDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
