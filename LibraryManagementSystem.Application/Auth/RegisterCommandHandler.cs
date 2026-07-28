using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IBranchRepository branchRepository,
        IPasswordHasher<User> passwordHasher,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _branchRepository = branchRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
        {
            throw new ConflictException($"Username '{request.Username}' is already taken.");
        }

        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            throw new ConflictException($"A user with email '{request.Email}' already exists.");
        }

        if (!await _userRepository.RoleExistsAsync(request.RoleId, cancellationToken))
        {
            throw new NotFoundException(nameof(Role), request.RoleId);
        }

        Branch? branch = null;
        if (request.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(request.BranchId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Branch), request.BranchId.Value);
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            RoleId = request.RoleId,
            BranchId = request.BranchId,
            Branch = branch,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        // Re-fetch so the Role navigation (needed by UserDto) is populated.
        var created = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

        _logger.LogInformation("User {UserId} ({Username}) registered with role {RoleId}", user.Id, user.Username, user.RoleId);

        return UserDto.FromEntity(created!);
    }
}
