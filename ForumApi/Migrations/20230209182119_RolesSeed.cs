using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumApi.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "roles",
                column: "name",
                values: new object[]
                {
                    "Admin",
                    "Moderator",
                    "User"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "name",
                keyValue: "Admin");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "name",
                keyValue: "Moderator");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "name",
                keyValue: "User");
        }
    }
}
