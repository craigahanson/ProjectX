using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectX.Data.EntityFrameworkCore.Migrations
{
    public partial class AddVersionSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Versions",
                columns: new[] { "Id", "Build", "CreatedDateTime", "Major", "Minor", "Revision", "UpdatedDateTime" },
                values: new object[] { 1L, 0, new DateTimeOffset(new DateTime(2021, 2, 6, 11, 5, 8, 856, DateTimeKind.Unspecified).AddTicks(2745), new TimeSpan(0, 0, 0, 0, 0)), 1, 0, 0, new DateTimeOffset(new DateTime(2021, 2, 6, 11, 5, 8, 856, DateTimeKind.Unspecified).AddTicks(2745), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
