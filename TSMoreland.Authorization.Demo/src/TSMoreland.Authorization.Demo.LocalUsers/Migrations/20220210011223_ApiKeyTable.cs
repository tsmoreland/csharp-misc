using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSMoreland.Authorization.Demo.LocalUsers.Migrations
{
    public partial class ApiKeyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoApiKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: false),
                    NotBefore = table.Column<long>(type: "INTEGER", nullable: false),
                    NotAfter = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoApiKeys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoApiKeys_ApiKey",
                table: "DemoApiKeys",
                column: "ApiKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoApiKeys");
        }
    }
}
