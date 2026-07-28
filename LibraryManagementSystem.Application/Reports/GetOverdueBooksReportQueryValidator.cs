using FluentValidation;

namespace LibraryManagementSystem.Application.Reports;

public class GetOverdueBooksReportQueryValidator : AbstractValidator<GetOverdueBooksReportQuery>
{
    public GetOverdueBooksReportQueryValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
    }
}
