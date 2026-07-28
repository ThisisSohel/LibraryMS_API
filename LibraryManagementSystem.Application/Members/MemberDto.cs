using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Members;

public record MemberDto(
    int Id,
    string FullName,
    string Email,
    string? Phone,
    string? Address,
    bool IsActive,
    int BranchId,
    string BranchName,
    DateTime CreatedAt)
{
    public static MemberDto FromEntity(Member member) => new(
        member.Id,
        member.FullName,
        member.Email,
        member.Phone,
        member.Address,
        member.IsActive,
        member.BranchId,
        member.Branch.Name,
        member.CreatedAt);
}
