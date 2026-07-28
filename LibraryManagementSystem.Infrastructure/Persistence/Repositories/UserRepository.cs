using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Users
            .Include(u => u.Role)
            .Include(u => u.Branch)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
        _context.Users
            .Include(u => u.Role)
            .Include(u => u.Branch)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken) =>
        _context.Users.AnyAsync(u => u.Username == username, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken) =>
        _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public Task<bool> RoleExistsAsync(int roleId, CancellationToken cancellationToken) =>
        _context.Roles.AnyAsync(r => r.Id == roleId, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken) =>
        await _context.Users.AddAsync(user, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
