using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectX.Data.EntityFrameworkCore.Migrations
{
    public partial class AddVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Versions",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Major = table.Column<int>("int", nullable: false),
                    Minor = table.Column<int>("int", nullable: false),
                    Build = table.Column<int>("int", nullable: false),
                    Revision = table.Column<int>("int", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>("datetimeoffset", nullable: false),
                    UpdatedDateTime = table.Column<DateTimeOffset>("datetimeoffset", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Versions", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Versions");
        }
    }
}