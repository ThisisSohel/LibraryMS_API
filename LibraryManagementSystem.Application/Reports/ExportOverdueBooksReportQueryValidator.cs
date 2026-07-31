using FluentValidation;

namespace LibraryManagementSystem.Application.Reports;

public class ExportOverdueBooksReportQueryValidator : AbstractValidator<ExportOverdueBooksReportQuery>
{
    public ExportOverdueBooksReportQueryValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
    }
}
