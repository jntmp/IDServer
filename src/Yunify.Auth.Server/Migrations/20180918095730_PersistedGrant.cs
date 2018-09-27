using Microsoft.EntityFrameworkCore.Migrations;

namespace Yunify.Auth.Server.Migrations
{
    public partial class PersistedGrant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    PersistedGrantData = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    SubjectId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedGrants");
        }
    }
}
