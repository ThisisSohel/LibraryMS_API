using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
{
    public void Configure(EntityTypeBuilder<BookCopy> builder)
    {
        builder.HasIndex(bc => new { bc.BookId, bc.BranchId }).IsUnique();

        builder.HasOne(bc => bc.Book)
            .WithMany(b => b.BookCopies)
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bc => bc.Branch)
            .WithMany(br => br.BookCopies)
            .HasForeignKey(bc => bc.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(bc => bc.CreatedBy).HasMaxLength(100);
        builder.Property(bc => bc.UpdatedBy).HasMaxLength(100);

        // This is the guards against lost updates when two requests borrow/return/fulfil against
        // the same (book, branch) stock row at the same time.
        builder.Property<uint>("xmin").IsRowVersion();
    }
}
