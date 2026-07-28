using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Members.Include(m => m.Branch).FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<(List<Member> Items, int TotalCount)> GetAllAsync(
        string? search, int? branchId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Members.Include(m => m.Branch).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(m =>
                EF.Functions.ILike(m.FullName, $"%{search}%") ||
                EF.Functions.ILike(m.Email, $"%{search}%"));
        }

        if (branchId.HasValue)
        {
            query = query.Where(m => m.BranchId == branchId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.FullName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken cancellationToken)
    {
        var query = _context.Members.Where(m => m.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(m => m.Id != excludeId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasDependentsAsync(int memberId, CancellationToken cancellationToken)
    {
        if (await _context.BorrowRecords.AnyAsync(br => br.MemberId == memberId, cancellationToken))
        {
            return true;
        }

        return await _context.Reservations.AnyAsync(r => r.MemberId == memberId, cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken) =>
        await _context.Members.AddAsync(member, cancellationToken);

    public void Update(Member member) => _context.Members.Update(member);

    public void Delete(Member member) => _context.Members.Remove(member);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
