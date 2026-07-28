using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class Book : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Publisher { get; set; }
    public int? PublishedYear { get; set; }

    public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
