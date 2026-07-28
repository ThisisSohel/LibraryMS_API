using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public record GetAllBorrowRecordsQuery(
    int? MemberId,
    int? BranchId,
    string? Status,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<BorrowRecordDto>>;
