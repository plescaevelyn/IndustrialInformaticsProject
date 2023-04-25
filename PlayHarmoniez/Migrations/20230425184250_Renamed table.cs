using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayHarmoniez.Migrations
{
    /// <inheritdoc />
    public partial class Renamedtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Playlist_Songs",
                table: "Playlist_Songs");

            migrationBuilder.RenameTable(
                name: "Playlist_Songs",
                newName: "PlaylistSongs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistSongs",
                table: "PlaylistSongs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistSongs",
                table: "PlaylistSongs");

            migrationBuilder.RenameTable(
                name: "PlaylistSongs",
                newName: "Playlist_Songs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playlist_Songs",
                table: "Playlist_Songs",
                column: "Id");
        }
    }
}
