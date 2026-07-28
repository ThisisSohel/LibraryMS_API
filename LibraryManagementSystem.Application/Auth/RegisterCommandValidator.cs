using FluentValidation;

namespace LibraryManagementSystem.Application.Auth;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(255).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.RoleId).GreaterThan(0);
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
    }
}
