using Microsoft.EntityFrameworkCore.Migrations;

namespace Holiday.Web.Data.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurentYearHolidaysNumber",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurentYearHolidaysNumber",
                table: "AspNetUsers");
        }
    }
}
