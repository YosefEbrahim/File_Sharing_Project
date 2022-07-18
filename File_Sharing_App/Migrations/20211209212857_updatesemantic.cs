using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Sharing_App.Migrations
{
    public partial class updatesemantic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContenrType",
                table: "Uploads",
                newName: "ContentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Uploads",
                newName: "ContenrType");
        }
    }
}
