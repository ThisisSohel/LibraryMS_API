using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public record CreateBranchCommand(
    string Name,
    string? Address,
    string? Phone,
    string? Email) : IRequest<BranchDto>;
