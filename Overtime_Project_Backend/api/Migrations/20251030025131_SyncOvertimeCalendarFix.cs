using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overtime_Project_Backend.Migrations
{
    /// <inheritdoc />
    public partial class SyncOvertimeCalendarFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_overtime_approvals_managers_ManagerId",
                table: "overtime_approvals");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OvertimeId",
                table: "notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_overtime_approvals_managers_ManagerId",
                table: "overtime_approvals",
                column: "ManagerId",
                principalTable: "managers",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_overtime_approvals_managers_ManagerId",
                table: "overtime_approvals");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "OvertimeId",
                table: "notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_overtime_approvals_managers_ManagerId",
                table: "overtime_approvals",
                column: "ManagerId",
                principalTable: "managers",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
