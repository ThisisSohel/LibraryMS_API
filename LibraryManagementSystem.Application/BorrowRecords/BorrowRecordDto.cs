using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.BorrowRecords;

public record BorrowRecordDto(
    int Id,
    int MemberId,
    string MemberName,
    int BookId,
    string BookTitle,
    int BranchId,
    string BranchName,
    int ProcessedByUserId,
    string ProcessedByUserName,
    DateTime BorrowDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    string Status,
    bool IsOverdue)
{
    public static BorrowRecordDto FromEntity(BorrowRecord record) => new(
        record.Id,
        record.MemberId,
        record.Member.FullName,
        record.BookCopy.BookId,
        record.BookCopy.Book.Title,
        record.BookCopy.BranchId,
        record.BookCopy.Branch.Name,
        record.ProcessedByUserId,
        record.ProcessedByUser.FullName,
        record.BorrowDate,
        record.DueDate,
        record.ReturnDate,
        record.Status.ToString(),
        record.Status == BorrowStatus.Borrowed && record.DueDate < DateTime.UtcNow);
}
