using FluentValidation;

namespace LibraryManagementSystem.Application.Books;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Isbn).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Category).MaximumLength(100);
        RuleFor(x => x.Publisher).MaximumLength(150);
        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1450, DateTime.UtcNow.Year)
            .When(x => x.PublishedYear.HasValue);
    }
}
