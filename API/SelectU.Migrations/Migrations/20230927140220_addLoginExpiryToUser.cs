using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelectU.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class addLoginExpiryToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LoginExpiry",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginExpiry",
                table: "Users");
        }
    }
}
