using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserDtoUserID",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UserDtoUserID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "UserDtoUserID",
                table: "Pictures");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserID",
                table: "Pictures",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserID",
                table: "Pictures",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserID",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UserID",
                table: "Pictures");

            migrationBuilder.AddColumn<int>(
                name: "UserDtoUserID",
                table: "Pictures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserDtoUserID",
                table: "Pictures",
                column: "UserDtoUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserDtoUserID",
                table: "Pictures",
                column: "UserDtoUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
