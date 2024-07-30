using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPhone = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PicturePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserEntityUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PicturePath);
                    table.ForeignKey(
                        name: "FK_Pictures_Users_UserEntityUserID",
                        column: x => x.UserEntityUserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserEntityUserID",
                table: "Pictures",
                column: "UserEntityUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPhone",
                table: "Users",
                column: "UserPhone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
