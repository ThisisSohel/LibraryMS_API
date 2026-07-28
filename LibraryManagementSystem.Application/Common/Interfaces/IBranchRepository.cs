using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(List<Branch> Items, int TotalCount)> GetAllAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken);
    Task<bool> HasDependentsAsync(int branchId, CancellationToken cancellationToken);
    Task AddAsync(Branch branch, CancellationToken cancellationToken);
    void Update(Branch branch);
    void Delete(Branch branch);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
