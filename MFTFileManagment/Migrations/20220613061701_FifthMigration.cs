using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFTFileManagment.Migrations
{
    public partial class FifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Files",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
