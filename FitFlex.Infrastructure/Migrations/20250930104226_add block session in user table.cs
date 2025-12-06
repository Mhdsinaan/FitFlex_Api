using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addblocksessioninusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentParticipants",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "MaxParticipants",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Sessions",
                newName: "TimeSlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSlot",
                table: "Sessions",
                newName: "StartTime");

            migrationBuilder.AddColumn<int>(
                name: "CurrentParticipants",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxParticipants",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
