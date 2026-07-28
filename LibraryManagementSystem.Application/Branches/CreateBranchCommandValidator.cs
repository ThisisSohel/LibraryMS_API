using FluentValidation;

namespace LibraryManagementSystem.Application.Branches;

public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Email).MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
