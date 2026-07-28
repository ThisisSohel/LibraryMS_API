using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.Property(br => br.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.HasOne(br => br.Member)
            .WithMany(m => m.BorrowRecords)
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(br => br.BookCopy)
            .WithMany(bc => bc.BorrowRecords)
            .HasForeignKey(br => br.BookCopyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(br => br.ProcessedByUser)
            .WithMany(u => u.ProcessedBorrowRecords)
            .HasForeignKey(br => br.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(br => br.CreatedBy).HasMaxLength(100);
        builder.Property(br => br.UpdatedBy).HasMaxLength(100);
    }
}
