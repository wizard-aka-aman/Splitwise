using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class List_of_string_userNames_GroupMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GroupMember_GroupMemberId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupMemberId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupMemberId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserNames",
                table: "GroupMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNames",
                table: "GroupMember");

            migrationBuilder.AddColumn<int>(
                name: "GroupMemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupMemberId",
                table: "AspNetUsers",
                column: "GroupMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GroupMember_GroupMemberId",
                table: "AspNetUsers",
                column: "GroupMemberId",
                principalTable: "GroupMember",
                principalColumn: "GroupMemberId");
        }
    }
}
