using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addusersession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSession_Users_UserId",
                table: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_UserSession_UserId",
                table: "UserSession");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserSession",
                newName: "TrainerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerID",
                table: "UserSession",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_UserId",
                table: "UserSession",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSession_Users_UserId",
                table: "UserSession",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
