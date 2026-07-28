using FluentValidation;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(x => x.BorrowRecordId).GreaterThan(0);
    }
}
