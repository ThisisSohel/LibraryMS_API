using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Domain.Entities;

public class BorrowRecord : AuditableEntity
{
    public int Id { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int BookCopyId { get; set; }
    public BookCopy BookCopy { get; set; } = null!;

    public int ProcessedByUserId { get; set; }
    public User ProcessedByUser { get; set; } = null!;
}
