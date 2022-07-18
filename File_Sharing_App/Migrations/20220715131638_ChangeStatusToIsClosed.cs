using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Sharing_App.Migrations
{
    public partial class ChangeStatusToIsClosed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Contact",
                newName: "IsClosed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Contact",
                newName: "Status");
        }
    }
}
