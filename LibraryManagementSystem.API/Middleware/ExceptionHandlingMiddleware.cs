using System.Net;
using System.Text.Json;
using LibraryManagementSystem.Application.Common.Exceptions;
using FluentValidation;

namespace LibraryManagementSystem.API.Middleware;

// Catches exceptions from anywhere in the request pipeline so controllers/handlers
// don't need their own try/catch blocks, and turns them into a consistent JSON error shape.
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message, null),
            ConflictException => (HttpStatusCode.Conflict, exception.Message, null),
            UnauthorizedException => (HttpStatusCode.Unauthorized, exception.Message, null),
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                "One or more validation errors occurred.",
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        else
        {
            _logger.LogWarning("{ExceptionType}: {Message}", exception.GetType().Name, exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new { message, errors };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
