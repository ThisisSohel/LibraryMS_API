using MediatR;

namespace LibraryManagementSystem.Application.Members;

public record UpdateMemberCommand(
    int Id,
    string FullName,
    string Email,
    string? Phone,
    string? Address,
    int BranchId,
    bool IsActive) : IRequest<MemberDto>;
