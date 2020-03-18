using Microsoft.EntityFrameworkCore.Migrations;

namespace Holiday.Web.Data.Migrations
{
    public partial class addNoDaysToHolidays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfDays",
                table: "HolidayRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfDays",
                table: "HolidayRequests");
        }
    }
}
