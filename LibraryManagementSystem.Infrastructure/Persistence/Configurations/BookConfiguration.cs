using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Isbn).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Category).HasMaxLength(100);
        builder.Property(b => b.Publisher).HasMaxLength(150);

        builder.HasIndex(b => b.Isbn).IsUnique();

        builder.Property(b => b.CreatedBy).HasMaxLength(100);
        builder.Property(b => b.UpdatedBy).HasMaxLength(100);
    }
}
