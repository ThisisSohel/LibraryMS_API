using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public class GetAllBranchesQueryHandler : IRequestHandler<GetAllBranchesQuery, PagedResult<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;

    public GetAllBranchesQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _branchRepository.GetAllAsync(
            request.Search, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = items.Select(BranchDto.FromEntity).ToList();

        return new PagedResult<BranchDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
