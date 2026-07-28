using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedUserToRealAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "email", "full_name", "password_hash", "role_id", "username" },
                values: new object[] { "admin@library.local", "System Administrator", "AQAAAAIAAYagAAAAEAERdo5mZJT/BQbJUB7DuxxKIfWRHXPKZpgIFrbM1n7DgQcZ3xnqBQ9dvNtk/caxCg==", 1, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "email", "full_name", "password_hash", "role_id", "username" },
                values: new object[] { "system.librarian@library.local", "System Librarian", "SEED-PLACEHOLDER-NOT-A-VALID-HASH", 2, "system.librarian" });
        }
    }
}
