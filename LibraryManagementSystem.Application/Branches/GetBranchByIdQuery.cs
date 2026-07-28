using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public record GetBranchByIdQuery(int Id) : IRequest<BranchDto>;
