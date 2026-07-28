using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultLibrarianUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Librarian" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "branch_id", "created_at", "created_by", "email", "full_name", "is_active", "password_hash", "role_id", "updated_at", "updated_by", "username" },
                values: new object[] { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "system.librarian@library.local", "System Librarian", true, "SEED-PLACEHOLDER-NOT-A-VALID-HASH", 2, null, null, "system.librarian" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
