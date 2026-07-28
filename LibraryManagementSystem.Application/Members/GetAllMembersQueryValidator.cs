using FluentValidation;

namespace LibraryManagementSystem.Application.Members;

public class GetAllMembersQueryValidator : AbstractValidator<GetAllMembersQuery>
{
    public GetAllMembersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
    }
}
