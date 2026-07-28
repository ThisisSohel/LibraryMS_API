using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(b => b.Name).IsRequired().HasMaxLength(150);
        builder.Property(b => b.Address).HasMaxLength(255);
        builder.Property(b => b.Phone).HasMaxLength(30);
        builder.Property(b => b.Email).HasMaxLength(255);

        builder.Property(b => b.CreatedBy).HasMaxLength(100);
        builder.Property(b => b.UpdatedBy).HasMaxLength(100);
    }
}
