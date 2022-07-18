using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Sharing_App.Migrations
{
    public partial class AddOriginalFileNameToUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Uploads");
        }
    }
}
