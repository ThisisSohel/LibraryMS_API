using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Domain.Entities;

public class Reservation : AuditableEntity
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
    public int QueuePosition { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
}
