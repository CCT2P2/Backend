using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class fuckingwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DISLIKE_ID",
                table: "USER",
                newName: "DISLIKE_I");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DISLIKE_I",
                table: "USER",
                newName: "DISLIKE_ID");
        }
    }
}
