using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public record GetBorrowRecordByIdQuery(int Id) : IRequest<BorrowRecordDto>;
