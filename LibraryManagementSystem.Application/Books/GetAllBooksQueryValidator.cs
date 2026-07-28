using FluentValidation;

namespace LibraryManagementSystem.Application.Books;

public class GetAllBooksQueryValidator : AbstractValidator<GetAllBooksQuery>
{
    public GetAllBooksQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
