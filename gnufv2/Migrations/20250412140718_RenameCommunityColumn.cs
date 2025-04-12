using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gnufv2.Migrations
{
    /// <inheritdoc />
    public partial class RenameCommunityColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community",
                columns: table => new
                {
                    COMMUNITY_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: false),
                    MEMBER_COUNT = table.Column<int>(type: "INTEGER", nullable: false),
                    IMG_PATH = table.Column<string>(type: "TEXT", nullable: true),
                    TAGS = table.Column<string>(type: "TEXT", nullable: false),
                    POST_ID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community", x => x.COMMUNITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "FEEDBACK",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WORKED = table.Column<string>(type: "TEXT", nullable: false),
                    DIDNT = table.Column<string>(type: "TEXT", nullable: false),
                    FEEDBACK = table.Column<string>(type: "TEXT", nullable: true),
                    RATING = table.Column<int>(type: "INTEGER", nullable: false),
                    TIMESTAMP = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEEDBACK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "POSTS",
                columns: table => new
                {
                    POST_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TITLE = table.Column<string>(type: "TEXT", nullable: false),
                    MAIN = table.Column<string>(type: "TEXT", nullable: false),
                    AUTHOR_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    COMMUNITY_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    TIMESTAMP = table.Column<long>(type: "INTEGER", nullable: false),
                    LIKES = table.Column<int>(type: "INTEGER", nullable: false),
                    DISLIKES = table.Column<int>(type: "INTEGER", nullable: false),
                    POST_ID_REF = table.Column<int>(type: "INTEGER", nullable: true),
                    COMMENT_FLAG = table.Column<bool>(type: "INTEGER", nullable: false),
                    COMMENT_CNT = table.Column<int>(type: "INTEGER", nullable: false),
                    COMMENTS = table.Column<string>(type: "TEXT", nullable: true),
                    IMG_PATH = table.Column<string>(type: "TEXT", nullable: true),
                    TAGS = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSTS", x => x.POST_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EMAIL = table.Column<string>(type: "TEXT", nullable: false),
                    USERNAME = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IMG_PATH = table.Column<string>(type: "TEXT", nullable: true),
                    POST_IDs = table.Column<string>(type: "TEXT", nullable: true),
                    COMMUNITY_IDs = table.Column<string>(type: "TEXT", nullable: true),
                    ADMIN = table.Column<int>(type: "INTEGER", nullable: false),
                    TAGS = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.USER_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community");

            migrationBuilder.DropTable(
                name: "FEEDBACK");

            migrationBuilder.DropTable(
                name: "POSTS");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
