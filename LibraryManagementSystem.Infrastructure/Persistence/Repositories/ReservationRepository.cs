using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Reservation> WithDetails() =>
        _context.Reservations
            .Include(r => r.Member)
            .Include(r => r.Book)
            .Include(r => r.Branch);

    public Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        WithDetails().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<(List<Reservation> Items, int TotalCount)> GetAllAsync(
        int? memberId, int? bookId, int? branchId, ReservationStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = WithDetails();

        if (memberId.HasValue)
        {
            query = query.Where(r => r.MemberId == memberId.Value);
        }

        if (bookId.HasValue)
        {
            query = query.Where(r => r.BookId == bookId.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(r => r.BranchId == branchId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(r => r.BookId).ThenBy(r => r.BranchId).ThenBy(r => r.QueuePosition)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> HasActiveReservationAsync(int memberId, int bookId, int branchId, CancellationToken cancellationToken) =>
        _context.Reservations.AnyAsync(r =>
            r.MemberId == memberId &&
            r.BookId == bookId &&
            r.BranchId == branchId &&
            r.Status == ReservationStatus.Pending,
            cancellationToken);

    public async Task<int> GetNextQueuePositionAsync(int bookId, int branchId, CancellationToken cancellationToken)
    {
        var maxPosition = await _context.Reservations
            .Where(r => r.BookId == bookId && r.BranchId == branchId && r.Status == ReservationStatus.Pending)
            .Select(r => (int?)r.QueuePosition)
            .MaxAsync(cancellationToken);

        return (maxPosition ?? 0) + 1;
    }

    public Task<Reservation?> GetFrontOfQueueAsync(int bookId, int branchId, CancellationToken cancellationToken) =>
        _context.Reservations
            .Where(r => r.BookId == bookId && r.BranchId == branchId && r.Status == ReservationStatus.Pending)
            .OrderBy(r => r.QueuePosition)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<Reservation>> GetQueueBehindAsync(int bookId, int branchId, int queuePosition, CancellationToken cancellationToken) =>
        await _context.Reservations
            .Where(r =>
                r.BookId == bookId &&
                r.BranchId == branchId &&
                r.Status == ReservationStatus.Pending &&
                r.QueuePosition > queuePosition)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken) =>
        await _context.Reservations.AddAsync(reservation, cancellationToken);

    public void Update(Reservation reservation) => _context.Reservations.Update(reservation);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
