using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayHarmoniez.Migrations
{
    /// <inheritdoc />
    public partial class added_constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Playlist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaylistId",
                table: "Playlist",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
