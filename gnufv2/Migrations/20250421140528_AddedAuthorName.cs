using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuthorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DISLIKE_ID",
                table: "USER",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LIKE_ID",
                table: "USER",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AUTHOR_NAME",
                table: "POSTS",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DISLIKE_ID",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "LIKE_ID",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "AUTHOR_NAME",
                table: "POSTS");
        }
    }
}
