using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayHarmoniez.Migrations
{
    /// <inheritdoc />
    public partial class addedallFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaylistName",
                table: "Playlist",
                newName: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_PlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_UserId",
                table: "Playlist",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedSongs_SongId",
                table: "LikedSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedSongs_UserId",
                table: "LikedSongs",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedSongs_Songs_SongId",
                table: "LikedSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedSongs_Users_UserId",
                table: "LikedSongs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlist_Users_UserId",
                table: "Playlist",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Playlist_PlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedSongs_Songs_SongId",
                table: "LikedSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedSongs_Users_UserId",
                table: "LikedSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlist_Users_UserId",
                table: "Playlist");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Playlist_PlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_PlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_Playlist_UserId",
                table: "Playlist");

            migrationBuilder.DropIndex(
                name: "IX_LikedSongs_SongId",
                table: "LikedSongs");

            migrationBuilder.DropIndex(
                name: "IX_LikedSongs_UserId",
                table: "LikedSongs");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "Playlist",
                newName: "PlaylistName");
        }
    }
}
