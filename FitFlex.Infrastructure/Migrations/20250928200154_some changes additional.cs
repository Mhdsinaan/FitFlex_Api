using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class somechangesadditional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlanID",
                table: "UserSubscriptionAddOn",
                newName: "PlanId");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalPlanId",
                table: "UserSubscriptionAddOn",
                type: "int",
                nullable: true
               );

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserSubscriptionAddOn",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AdditionalPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMonth = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalPlan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionAddOn_AdditionalPlanId",
                table: "UserSubscriptionAddOn",
                column: "AdditionalPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptionAddOn_AdditionalPlan_AdditionalPlanId",
                table: "UserSubscriptionAddOn",
                column: "AdditionalPlanId",
                principalTable: "AdditionalPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptionAddOn_AdditionalPlan_AdditionalPlanId",
                table: "UserSubscriptionAddOn");

            migrationBuilder.DropTable(
                name: "AdditionalPlan");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptionAddOn_AdditionalPlanId",
                table: "UserSubscriptionAddOn");

            migrationBuilder.DropColumn(
                name: "AdditionalPlanId",
                table: "UserSubscriptionAddOn");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserSubscriptionAddOn");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "UserSubscriptionAddOn",
                newName: "PlanID");
        }
    }
}
