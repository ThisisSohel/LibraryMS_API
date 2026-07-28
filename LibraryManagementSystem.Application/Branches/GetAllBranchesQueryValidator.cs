using FluentValidation;

namespace LibraryManagementSystem.Application.Branches;

public class GetAllBranchesQueryValidator : AbstractValidator<GetAllBranchesQuery>
{
    public GetAllBranchesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
