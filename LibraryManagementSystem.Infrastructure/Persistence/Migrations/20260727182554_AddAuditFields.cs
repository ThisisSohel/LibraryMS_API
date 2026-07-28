using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "reservations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "reservations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "reservations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "reservations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "members",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "branches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "branches",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "branches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "borrow_records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "borrow_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "borrow_records",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "borrow_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "books",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "books",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "books",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "book_copies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "book_copies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "book_copies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "book_copies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "members");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "members");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "members");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "branches");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "branches");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "branches");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "borrow_records");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "borrow_records");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "borrow_records");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "borrow_records");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "books");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "books");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "books");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "book_copies");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "book_copies");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "book_copies");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "book_copies");
        }
    }
}
