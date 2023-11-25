using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PontoLegal.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimeClockNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClockNotification_Employees_EmployeeId",
                table: "TimeClockNotification");

            migrationBuilder.DropIndex(
                name: "IX_TimeClockNotification_EmployeeId",
                table: "TimeClockNotification");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TimeClockNotification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "TimeClockNotification",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClockNotification_EmployeeId",
                table: "TimeClockNotification",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClockNotification_Employees_EmployeeId",
                table: "TimeClockNotification",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
