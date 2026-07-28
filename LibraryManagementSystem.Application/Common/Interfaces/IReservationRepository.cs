using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(List<Reservation> Items, int TotalCount)> GetAllAsync(
        int? memberId, int? bookId, int? branchId, ReservationStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> HasActiveReservationAsync(int memberId, int bookId, int branchId, CancellationToken cancellationToken);
    Task<int> GetNextQueuePositionAsync(int bookId, int branchId, CancellationToken cancellationToken);
    Task<Reservation?> GetFrontOfQueueAsync(int bookId, int branchId, CancellationToken cancellationToken);
    Task<List<Reservation>> GetQueueBehindAsync(int bookId, int branchId, int queuePosition, CancellationToken cancellationToken);
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken);
    void Update(Reservation reservation);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
