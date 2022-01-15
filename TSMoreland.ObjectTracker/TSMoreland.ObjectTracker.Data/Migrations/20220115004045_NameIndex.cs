using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSMoreland.ObjectTracker.Data.Migrations
{
    public partial class NameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Objects_Name",
                table: "Objects",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objects_Name",
                table: "Objects");
        }
    }
}
