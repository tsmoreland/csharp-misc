using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSMoreland.Authorization.Demo.LocalUsers.Migrations
{
    public partial class ApiKeyNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DemoApiKeys_Name",
                table: "DemoApiKeys",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DemoApiKeys_Name",
                table: "DemoApiKeys");
        }
    }
}
