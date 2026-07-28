using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public record GetAllBranchesQuery(string? Search, int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<BranchDto>>;
