using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Auth;

public record UserDto(
    int Id,
    string Username,
    string Email,
    string FullName,
    string Role,
    int? BranchId,
    string? BranchName,
    bool IsActive)
{
    public static UserDto FromEntity(User user) => new(
        user.Id,
        user.Username,
        user.Email,
        user.FullName,
        user.Role.Name,
        user.BranchId,
        user.Branch?.Name,
        user.IsActive);
}
