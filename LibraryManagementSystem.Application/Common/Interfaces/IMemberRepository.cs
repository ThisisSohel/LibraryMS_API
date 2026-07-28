using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(List<Member> Items, int TotalCount)> GetAllAsync(string? search, int? branchId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken cancellationToken);
    Task<bool> HasDependentsAsync(int memberId, CancellationToken cancellationToken);
    Task AddAsync(Member member, CancellationToken cancellationToken);
    void Update(Member member);
    void Delete(Member member);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
