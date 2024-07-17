using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class IdToLikeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Likes",
                newName: "LikeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LikeId",
                table: "Likes",
                newName: "Id");
        }
    }
}
