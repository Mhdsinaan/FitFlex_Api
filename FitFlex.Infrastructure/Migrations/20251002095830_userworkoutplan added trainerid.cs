using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userworkoutplanaddedtrainerid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add TrainerId as nullable (no default value)
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "UserWorkoutAssignment",
                type: "int",
                nullable: true);

            // Create index for faster lookups
            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutAssignment_TrainerId",
                table: "UserWorkoutAssignment",
                column: "TrainerId");

            // Add foreign key to Trainers table
            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkoutAssignment_Trainers_TrainerId",
                table: "UserWorkoutAssignment",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict // Prevent cascade delete
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkoutAssignment_Trainers_TrainerId",
                table: "UserWorkoutAssignment");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkoutAssignment_TrainerId",
                table: "UserWorkoutAssignment");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "UserWorkoutAssignment");
        }
    }
}
