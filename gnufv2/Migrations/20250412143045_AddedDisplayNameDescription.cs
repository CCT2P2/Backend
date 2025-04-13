using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class AddedDisplayNameDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_community",
                table: "community");

            migrationBuilder.RenameTable(
                name: "community",
                newName: "COMMUNITIES");

            migrationBuilder.AddColumn<string>(
                name: "DESCRIPTION",
                table: "USER",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DISPLAY_NAME",
                table: "USER",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_COMMUNITIES",
                table: "COMMUNITIES",
                column: "COMMUNITY_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_COMMUNITIES",
                table: "COMMUNITIES");

            migrationBuilder.DropColumn(
                name: "DESCRIPTION",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "DISPLAY_NAME",
                table: "USER");

            migrationBuilder.RenameTable(
                name: "COMMUNITIES",
                newName: "community");

            migrationBuilder.AddPrimaryKey(
                name: "PK_community",
                table: "community",
                column: "COMMUNITY_ID");
        }
    }
}
