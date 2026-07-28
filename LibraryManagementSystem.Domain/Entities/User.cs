using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class User : AuditableEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public ICollection<BorrowRecord> ProcessedBorrowRecords { get; set; } = new List<BorrowRecord>();
}
