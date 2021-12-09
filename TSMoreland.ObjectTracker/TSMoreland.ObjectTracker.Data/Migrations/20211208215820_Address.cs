using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSMoreland.ObjectTracker.Data.Migrations
{
    public partial class Address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Objects",
                type: "TEXT",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "Objects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Objects",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Objects",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Objects");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Objects",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldRowVersion: true);
        }
    }
}
