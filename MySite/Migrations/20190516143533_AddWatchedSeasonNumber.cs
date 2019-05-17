using Microsoft.EntityFrameworkCore.Migrations;

namespace MySite.Migrations
{
    public partial class AddWatchedSeasonNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonNumber",
                table: "Watched",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonNumber",
                table: "Watched");
        }
    }
}
