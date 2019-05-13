using Microsoft.EntityFrameworkCore.Migrations;

namespace MySite.Migrations
{
    public partial class AddDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Watched",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Read",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Watched");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Read");
        }
    }
}
