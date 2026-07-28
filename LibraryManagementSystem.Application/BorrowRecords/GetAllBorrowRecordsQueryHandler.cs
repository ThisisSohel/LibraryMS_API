using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Models;
using LibraryManagementSystem.Domain.Enums;
using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class GetAllBorrowRecordsQueryHandler : IRequestHandler<GetAllBorrowRecordsQuery, PagedResult<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetAllBorrowRecordsQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<PagedResult<BorrowRecordDto>> Handle(GetAllBorrowRecordsQuery request, CancellationToken cancellationToken)
    {
        BorrowStatus? status = string.IsNullOrWhiteSpace(request.Status)
            ? null
            : Enum.Parse<BorrowStatus>(request.Status, ignoreCase: true);

        var (items, totalCount) = await _borrowRecordRepository.GetAllAsync(
            request.MemberId, request.BranchId, status, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = items.Select(BorrowRecordDto.FromEntity).ToList();

        return new PagedResult<BorrowRecordDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
