using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayHarmoniez.Migrations
{
    /// <inheritdoc />
    public partial class notxd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescriptionXD",
                table: "Songs",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Songs",
                newName: "DescriptionXD");
        }
    }
}
