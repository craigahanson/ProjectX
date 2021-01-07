using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectX.Data.EntityFrameworkCore.Migrations
{
    public partial class AddVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Major = table.Column<int>(type: "int", nullable: false),
                    Minor = table.Column<int>(type: "int", nullable: false),
                    Build = table.Column<int>(type: "int", nullable: false),
                    Revision = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Versions");
        }
    }
}
