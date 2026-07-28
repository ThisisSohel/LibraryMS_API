using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class Member : AuditableEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
