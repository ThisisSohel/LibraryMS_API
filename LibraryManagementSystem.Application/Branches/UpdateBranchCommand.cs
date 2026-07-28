using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public record UpdateBranchCommand(
    int Id,
    string Name,
    string? Address,
    string? Phone,
    string? Email,
    bool IsActive) : IRequest<BranchDto>;
