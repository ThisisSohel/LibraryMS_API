using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public record BorrowBookCommand(
    int MemberId,
    int BookId,
    int BranchId,
    int ProcessedByUserId) : IRequest<BorrowRecordDto>;
