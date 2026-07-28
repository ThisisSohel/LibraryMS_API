using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> RoleExistsAsync(int roleId, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
