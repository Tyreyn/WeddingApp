using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserEntityUserID",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "UserEntityUserID",
                table: "Pictures",
                newName: "UserDtoUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_UserEntityUserID",
                table: "Pictures",
                newName: "IX_Pictures_UserDtoUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserDtoUserID",
                table: "Pictures",
                column: "UserDtoUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserDtoUserID",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "UserDtoUserID",
                table: "Pictures",
                newName: "UserEntityUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_UserDtoUserID",
                table: "Pictures",
                newName: "IX_Pictures_UserEntityUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserEntityUserID",
                table: "Pictures",
                column: "UserEntityUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
