using LibraryManagementSystem.Application.BorrowRecords;
using MediatR;

namespace LibraryManagementSystem.Application.Reservations;

public record FulfillReservationCommand(int Id, int ProcessedByUserId) : IRequest<BorrowRecordDto>;
