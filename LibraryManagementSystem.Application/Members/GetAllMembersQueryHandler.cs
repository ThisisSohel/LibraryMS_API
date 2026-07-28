using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Members;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, PagedResult<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;

    public GetAllMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<PagedResult<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _memberRepository.GetAllAsync(
            request.Search, request.BranchId, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = items.Select(MemberDto.FromEntity).ToList();

        return new PagedResult<MemberDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
