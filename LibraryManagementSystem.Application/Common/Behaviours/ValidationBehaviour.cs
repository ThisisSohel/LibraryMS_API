using FluentValidation;
using MediatR;

namespace LibraryManagementSystem.Application.Common.Behaviours;

// Runs before every command/query that has a matching FluentValidation validator registered.
// If validation fails, it throws FluentValidation.ValidationException, which the API's
// exception-handling middleware turns into a 400 response with the list of errors.
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
