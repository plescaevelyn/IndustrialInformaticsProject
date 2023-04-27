using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayHarmoniez.Migrations
{
    /// <inheritdoc />
    public partial class XD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Songs",
                newName: "DescriptionXD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescriptionXD",
                table: "Songs",
                newName: "Description");
        }
    }
}
