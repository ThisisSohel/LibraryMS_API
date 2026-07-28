using FluentValidation;

namespace LibraryManagementSystem.Application.Reservations;

public class FulfillReservationCommandValidator : AbstractValidator<FulfillReservationCommand>
{
    public FulfillReservationCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ProcessedByUserId).GreaterThan(0);
    }
}
