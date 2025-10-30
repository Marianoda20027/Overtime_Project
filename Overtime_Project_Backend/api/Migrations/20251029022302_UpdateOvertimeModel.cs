using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overtime_Project_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOvertimeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "human_resources",
                columns: table => new
                {
                    HumanResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_human_resources", x => x.HumanResourceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_human_resources_Email",
                table: "human_resources",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "human_resources");
        }
    }
}
