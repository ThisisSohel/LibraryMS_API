using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncOptimisticConcurrencyToken : Migration
    {
        // "xmin" is a Postgres system column that already exists on every table — EF Core's
        // scaffolder wants to AddColumn/DropColumn it, but that fails ("column name "xmin"
        // conflicts with a system column name"). Nothing to migrate; this only teaches EF's
        // model snapshot that book_copies now uses xmin as its concurrency token.

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
