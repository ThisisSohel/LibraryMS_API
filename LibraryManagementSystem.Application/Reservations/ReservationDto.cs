using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Reservations;

public record ReservationDto(
    int Id,
    int MemberId,
    string MemberName,
    int BookId,
    string BookTitle,
    int BranchId,
    string BranchName,
    DateTime ReservationDate,
    string Status,
    int QueuePosition)
{
    public static ReservationDto FromEntity(Reservation reservation) => new(
        reservation.Id,
        reservation.MemberId,
        reservation.Member.FullName,
        reservation.BookId,
        reservation.Book.Title,
        reservation.BranchId,
        reservation.Branch.Name,
        reservation.ReservationDate,
        reservation.Status.ToString(),
        reservation.QueuePosition);
}
