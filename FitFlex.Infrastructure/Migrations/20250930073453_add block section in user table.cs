using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addblocksectioninusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedAt",
                table: "UserSubscriptions");

            migrationBuilder.AddColumn<bool>(
                name: "Isblock",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isblock",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);
        }
    }
}
