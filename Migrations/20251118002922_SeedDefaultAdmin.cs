using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "IsApproved", "Password", "Username" },
                values: new object[] { 1, true, "$2a$11$SaUg7r1FkVsEcJkADS0I4e/Nnitfsq8GYU5F02G5AEi9IWy9WOx8i", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
