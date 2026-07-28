using FluentValidation;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class GetAllBorrowRecordsQueryValidator : AbstractValidator<GetAllBorrowRecordsQuery>
{
    public GetAllBorrowRecordsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MemberId).GreaterThan(0).When(x => x.MemberId.HasValue);
        RuleFor(x => x.BranchId).GreaterThan(0).When(x => x.BranchId.HasValue);
        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse<BorrowStatus>(status, ignoreCase: true, out _))
            .WithMessage("Status must be one of: Borrowed, Returned, Overdue.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }
}
