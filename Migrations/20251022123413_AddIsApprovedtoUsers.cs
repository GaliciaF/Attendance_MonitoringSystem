using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedtoUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Students",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Admins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Admins");
        }
    }
}
