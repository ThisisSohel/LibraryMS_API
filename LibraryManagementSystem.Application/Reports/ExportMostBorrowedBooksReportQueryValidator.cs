using FluentValidation;

namespace LibraryManagementSystem.Application.Reports;

public class ExportMostBorrowedBooksReportQueryValidator : AbstractValidator<ExportMostBorrowedBooksReportQuery>
{
    public ExportMostBorrowedBooksReportQueryValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
        RuleFor(x => x.Top).InclusiveBetween(1, 100);
    }
}
