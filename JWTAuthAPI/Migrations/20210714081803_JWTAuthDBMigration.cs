using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTAuthAPI.Migrations
{
    public partial class JWTAuthDBMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "RefreshToken", "RefreshTokenExpiryTime", "UserName" },
                values: new object[] { 1L, "123", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "caner" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "RefreshToken", "RefreshTokenExpiryTime", "UserName" },
                values: new object[] { 2L, "123", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
