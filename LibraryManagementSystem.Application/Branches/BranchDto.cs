using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Branches;

public record BranchDto(
    int Id,
    string Name,
    string? Address,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTime CreatedAt)
{
    public static BranchDto FromEntity(Branch branch) => new(
        branch.Id,
        branch.Name,
        branch.Address,
        branch.Phone,
        branch.Email,
        branch.IsActive,
        branch.CreatedAt);
}
