using FluentValidation;

namespace LibraryManagementSystem.Application.Reservations;

public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
