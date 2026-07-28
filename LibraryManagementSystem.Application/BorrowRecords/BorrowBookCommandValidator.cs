using FluentValidation;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(x => x.MemberId).GreaterThan(0);
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.ProcessedByUserId).GreaterThan(0);
    }
}
