namespace LibraryManagementSystem.Application.Reports;

public record OverdueBookDto(
    int BorrowRecordId,
    int MemberId,
    string MemberName,
    string MemberEmail,
    int BookId,
    string BookTitle,
    int BranchId,
    string BranchName,
    DateTime BorrowDate,
    DateTime DueDate,
    int DaysOverdue);
