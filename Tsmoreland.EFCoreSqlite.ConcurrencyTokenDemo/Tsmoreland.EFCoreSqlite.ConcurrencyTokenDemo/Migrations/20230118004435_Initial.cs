using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tsmoreland.EFCoreSqlite.ConcurrencyTokenDemo.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AddressHouseNumber = table.Column<int>(name: "Address_HouseNumber", type: "INTEGER", nullable: false),
                    AddressStreetName = table.Column<string>(name: "Address_StreetName", type: "TEXT", maxLength: 100, nullable: false),
                    AddressPostCode = table.Column<string>(name: "Address_PostCode", type: "TEXT", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TimeStamp = table.Column<string>(type: "TEXT", rowVersion: true, nullable: false, defaultValueSql: "hex(randomblob(8))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });
            migrationBuilder.Sql(
                """
                CREATE TRIGGER SetPeopleTimestampOnUpdate
                AFTER UPDATE ON People
                BEGIN
                    UPDATE People
                    SET Timestamp = hex(randomblob(8))
                    WHERE rowid = NEW.rowid;
                END
                """);
            migrationBuilder.Sql(
                """
                CREATE TRIGGER SetPeopleTimestampOnInsert
                AFTER INSERT ON People
                BEGIN
                    UPDATE People
                    SET Timestamp = hex(randomblob(8))
                    WHERE rowid = NEW.rowid;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
