using FluentValidation;

namespace LibraryManagementSystem.Application.Members;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(255).EmailAddress();
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Address).MaximumLength(255);
        RuleFor(x => x.BranchId).GreaterThan(0);
    }
}
