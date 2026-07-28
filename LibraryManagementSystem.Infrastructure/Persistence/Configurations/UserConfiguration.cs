using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(150);

        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Branch)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(u => u.CreatedBy).HasMaxLength(100);
        builder.Property(u => u.UpdatedBy).HasMaxLength(100);

        // Bootstrap Admin account — the only way to create further staff users is via
        // POST /api/auth/register, which is Admin-only, so at least one real loginable Admin
        // must exist from the start. Default password is "Admin@123" (documented in README);
        // rotate it immediately in any real deployment. Hash generated once with
        // PasswordHasher<T> and hardcoded here so the seed stays deterministic across migrations.
        builder.HasData(new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@library.local",
            PasswordHash = "AQAAAAIAAYagAAAAEAERdo5mZJT/BQbJUB7DuxxKIfWRHXPKZpgIFrbM1n7DgQcZ3xnqBQ9dvNtk/caxCg==",
            FullName = "System Administrator",
            RoleId = 1,
            BranchId = null,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
