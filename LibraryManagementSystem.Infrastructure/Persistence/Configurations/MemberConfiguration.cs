using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(255);
        builder.Property(m => m.Phone).HasMaxLength(30);
        builder.Property(m => m.Address).HasMaxLength(255);

        builder.HasIndex(m => m.Email).IsUnique();

        builder.HasOne(m => m.Branch)
            .WithMany(b => b.Members)
            .HasForeignKey(m => m.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(m => m.CreatedBy).HasMaxLength(100);
        builder.Property(m => m.UpdatedBy).HasMaxLength(100);
    }
}
