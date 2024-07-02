using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquessToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserPhone",
                table: "Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPhone",
                table: "Users",
                column: "UserPhone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserPhone",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserPhone",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
