using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class AddedHashing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SALT",
                table: "USER",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SALT",
                table: "USER");
        }
    }
}
