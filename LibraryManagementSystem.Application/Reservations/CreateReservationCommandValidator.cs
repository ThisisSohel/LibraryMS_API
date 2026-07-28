using FluentValidation;

namespace LibraryManagementSystem.Application.Reservations;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.MemberId).GreaterThan(0);
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.BranchId).GreaterThan(0);
    }
}
