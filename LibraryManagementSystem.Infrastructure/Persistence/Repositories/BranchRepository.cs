using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Branch?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Branches.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<(List<Branch> Items, int TotalCount)> GetAllAsync(
        string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Branches.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                EF.Functions.ILike(b.Name, $"%{search}%") ||
                (b.Address != null && EF.Functions.ILike(b.Address, $"%{search}%")));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(b => b.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken)
    {
        var query = _context.Branches.Where(b => b.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Id != excludeId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasDependentsAsync(int branchId, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.BranchId == branchId, cancellationToken))
        {
            return true;
        }

        if (await _context.Members.AnyAsync(m => m.BranchId == branchId, cancellationToken))
        {
            return true;
        }

        if (await _context.BookCopies.AnyAsync(bc => bc.BranchId == branchId, cancellationToken))
        {
            return true;
        }

        return await _context.Reservations.AnyAsync(r => r.BranchId == branchId, cancellationToken);
    }

    public async Task AddAsync(Branch branch, CancellationToken cancellationToken) =>
        await _context.Branches.AddAsync(branch, cancellationToken);

    public void Update(Branch branch) => _context.Branches.Update(branch);

    public void Delete(Branch branch) => _context.Branches.Remove(branch);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
