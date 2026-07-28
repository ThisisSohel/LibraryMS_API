using FluentValidation;

namespace LibraryManagementSystem.Application.Books;

public class AddBookCopiesCommandValidator : AbstractValidator<AddBookCopiesCommand>
{
    public AddBookCopiesCommandValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
