using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotoEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Members_MemberId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Photos",
                newName: "MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_MemberId",
                table: "Photos",
                newName: "IX_Photos_MemberID");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Members_MemberID",
                table: "Photos",
                column: "MemberID",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Members_MemberID",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "Photos",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_MemberID",
                table: "Photos",
                newName: "IX_Photos_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Members_MemberId",
                table: "Photos",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
