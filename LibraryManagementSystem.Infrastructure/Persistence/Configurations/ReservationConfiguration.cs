using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.HasOne(r => r.Member)
            .WithMany(m => m.Reservations)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Book)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Branch)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.CreatedBy).HasMaxLength(100);
        builder.Property(r => r.UpdatedBy).HasMaxLength(100);
    }
}
