using MediatR;

namespace LibraryManagementSystem.Application.Members;

public record CreateMemberCommand(
    string FullName,
    string Email,
    string? Phone,
    string? Address,
    int BranchId) : IRequest<MemberDto>;
