using MediatR;

namespace LibraryManagementSystem.Application.Auth;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FullName,
    int RoleId,
    int? BranchId) : IRequest<UserDto>;
