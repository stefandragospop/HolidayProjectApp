using Microsoft.EntityFrameworkCore.Migrations;

namespace Holiday.Web.Data.Migrations
{
    public partial class update_holiday_request : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "HolidayRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequests_EmployeeId",
                table: "HolidayRequests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayRequests_AspNetUsers_EmployeeId",
                table: "HolidayRequests",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolidayRequests_AspNetUsers_EmployeeId",
                table: "HolidayRequests");

            migrationBuilder.DropIndex(
                name: "IX_HolidayRequests_EmployeeId",
                table: "HolidayRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "HolidayRequests");
        }
    }
}
