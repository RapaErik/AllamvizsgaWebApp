using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class insert_setpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DailySetpoint",
                table: "Rooms",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NightlySetpoint",
                table: "Rooms",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailySetpoint",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NightlySetpoint",
                table: "Rooms");
        }
    }
}
