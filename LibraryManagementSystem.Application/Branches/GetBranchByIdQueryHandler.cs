using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, BranchDto>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchByIdQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<BranchDto> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.Id);

        return BranchDto.FromEntity(branch);
    }
}
