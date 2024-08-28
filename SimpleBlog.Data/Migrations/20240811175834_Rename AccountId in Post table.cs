using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccountIdinPosttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_AccountOwnerAccountId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AccountReceiverId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "AccountOwnerAccountId",
                table: "Posts",
                newName: "OwnerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AccountOwnerAccountId",
                table: "Posts",
                newName: "IX_Posts_OwnerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_OwnerAccountId",
                table: "Posts",
                column: "OwnerAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_OwnerAccountId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "OwnerAccountId",
                table: "Posts",
                newName: "AccountOwnerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_OwnerAccountId",
                table: "Posts",
                newName: "IX_Posts_AccountOwnerAccountId");

            migrationBuilder.AddColumn<int>(
                name: "AccountReceiverId",
                table: "Likes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_AccountOwnerAccountId",
                table: "Posts",
                column: "AccountOwnerAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
