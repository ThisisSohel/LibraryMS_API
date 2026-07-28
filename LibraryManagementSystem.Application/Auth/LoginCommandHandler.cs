using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        // Deliberately the same message whether the username doesn't exist or the password is
        // wrong, so a caller can't use this endpoint to enumerate valid usernames.
        if (user is null || !user.IsActive || !VerifyPassword(user, request.Password))
        {
            _logger.LogWarning("Failed login attempt for username {Username}", request.Username);
            throw new UnauthorizedException("Invalid username or password.");
        }

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation("User {UserId} ({Username}) logged in", user.Id, user.Username);

        return new LoginResultDto(token, expiresAt, UserDto.FromEntity(user));
    }

    private bool VerifyPassword(User user, string password)
    {
        try
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        }
        catch (FormatException)
        {
            // Stored hash isn't in the expected format (e.g. a seed placeholder) — treat as a failed login.
            return false;
        }
    }
}
