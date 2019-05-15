using Microsoft.EntityFrameworkCore.Migrations;

namespace MySite.Migrations
{
    public partial class AddWatchedType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WatchedType",
                table: "Watched",
                nullable: false,
                defaultValue: 0);

            //All exisiting watched have been movies
            migrationBuilder.Sql("UPDATE public.\"Watched\" SET \"WatchedType\" = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatchedType",
                table: "Watched");
        }
    }
}
