namespace LibraryManagementSystem.Application.Reports;

public record MostBorrowedBookDto(int BookId, string Title, string Author, int BorrowCount);
