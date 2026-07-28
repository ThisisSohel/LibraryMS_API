using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Members;

public record GetAllMembersQuery(
    string? Search,
    int? BranchId,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<MemberDto>>;
