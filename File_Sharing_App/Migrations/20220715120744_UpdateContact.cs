using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Sharing_App.Migrations
{
    public partial class UpdateContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Contact",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Contact",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contact");
        }
    }
}
