using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class Branch : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
