using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3_Card_Poker.Migrations
{
    public partial class Playeroutcome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Outcome",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Players");
        }
    }
}
