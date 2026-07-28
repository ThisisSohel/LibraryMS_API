using FluentValidation;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.Reservations;

public class GetAllReservationsQueryValidator : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MemberId).GreaterThan(0).When(x => x.MemberId.HasValue);
        RuleFor(x => x.BookId).GreaterThan(0).When(x => x.BookId.HasValue);
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse<ReservationStatus>(status, ignoreCase: true, out _))
            .WithMessage("Status must be one of: Pending, Ready, Fulfilled, Cancelled.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }
}
