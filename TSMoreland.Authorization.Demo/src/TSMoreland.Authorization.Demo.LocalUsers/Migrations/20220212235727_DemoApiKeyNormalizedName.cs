using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSMoreland.Authorization.Demo.LocalUsers.Migrations
{
    public partial class DemoApiKeyNormalizedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "DemoApiKeys",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "DemoApiKeys");
        }
    }
}
