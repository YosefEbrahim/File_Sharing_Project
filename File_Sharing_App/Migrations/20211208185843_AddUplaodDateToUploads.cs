using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Sharing_App.Migrations
{
    public partial class AddUplaodDateToUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Uploads",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Uploads");
        }
    }
}
