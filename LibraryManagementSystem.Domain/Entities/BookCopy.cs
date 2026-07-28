using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class BookCopy : AuditableEntity
{
    public int Id { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
