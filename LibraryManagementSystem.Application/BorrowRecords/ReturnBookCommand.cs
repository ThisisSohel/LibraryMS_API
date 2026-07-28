using MediatR;

namespace LibraryManagementSystem.Application.BorrowRecords;

public record ReturnBookCommand(int BorrowRecordId) : IRequest<BorrowRecordDto>;
