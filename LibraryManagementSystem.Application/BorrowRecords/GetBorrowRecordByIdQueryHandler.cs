using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public class GetBorrowRecordByIdQueryHandler : IRequestHandler<GetBorrowRecordByIdQuery, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetBorrowRecordByIdQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<BorrowRecordDto> Handle(GetBorrowRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(BorrowRecord), request.Id);

        return BorrowRecordDto.FromEntity(borrowRecord);
    }
}
