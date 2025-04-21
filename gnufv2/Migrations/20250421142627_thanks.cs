using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class thanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DISLIKE_I",
                table: "USER",
                newName: "DISLIKE_ID");

            migrationBuilder.AddColumn<string>(
                name: "COMMENT_ID",
                table: "USER",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COMMENT_ID",
                table: "USER");

            migrationBuilder.RenameColumn(
                name: "DISLIKE_ID",
                table: "USER",
                newName: "DISLIKE_I");
        }
    }
}
