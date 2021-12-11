using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Salmpled.Migrations
{
    public partial class CutDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SamplePackSamplePackGenre_SamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SamplePackSamplePackGenre_SamplePack_SamplePackId",
                table: "SamplePackSamplePackGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleSamplePlaylist_Sample_SampleId",
                table: "SampleSamplePlaylist");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleSamplePlaylist_SamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylist");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "SampleLike");

            migrationBuilder.DropTable(
                name: "SamplePackLike");

            migrationBuilder.DropTable(
                name: "SamplePlaylistLike");

            migrationBuilder.DropTable(
                name: "SamplePlaylistSamplePlaylistGenre");

            migrationBuilder.DropTable(
                name: "SampleSampleTag");

            migrationBuilder.DropTable(
                name: "SamplePlaylistGenre");

            migrationBuilder.DropTable(
                name: "SampleTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SampleSamplePlaylist",
                table: "SampleSamplePlaylist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SamplePackSamplePackGenre",
                table: "SamplePackSamplePackGenre");

            migrationBuilder.RenameTable(
                name: "SampleSamplePlaylist",
                newName: "SampleSamplePlaylists");

            migrationBuilder.RenameTable(
                name: "SamplePackSamplePackGenre",
                newName: "SamplePackSamplePackGenres");

            migrationBuilder.RenameIndex(
                name: "IX_SampleSamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylists",
                newName: "IX_SampleSamplePlaylists_SamplePlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_SamplePackSamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenres",
                newName: "IX_SamplePackSamplePackGenres_SamplePackGenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SampleSamplePlaylists",
                table: "SampleSamplePlaylists",
                columns: new[] { "SampleId", "SamplePlaylistId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamplePackSamplePackGenres",
                table: "SamplePackSamplePackGenres",
                columns: new[] { "SamplePackId", "SamplePackGenreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SamplePackSamplePackGenres_SamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenres",
                column: "SamplePackGenreId",
                principalTable: "SamplePackGenre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SamplePackSamplePackGenres_SamplePack_SamplePackId",
                table: "SamplePackSamplePackGenres",
                column: "SamplePackId",
                principalTable: "SamplePack",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleSamplePlaylists_Sample_SampleId",
                table: "SampleSamplePlaylists",
                column: "SampleId",
                principalTable: "Sample",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleSamplePlaylists_SamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylists",
                column: "SamplePlaylistId",
                principalTable: "SamplePlaylist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SamplePackSamplePackGenres_SamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_SamplePackSamplePackGenres_SamplePack_SamplePackId",
                table: "SamplePackSamplePackGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleSamplePlaylists_Sample_SampleId",
                table: "SampleSamplePlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleSamplePlaylists_SamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SampleSamplePlaylists",
                table: "SampleSamplePlaylists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SamplePackSamplePackGenres",
                table: "SamplePackSamplePackGenres");

            migrationBuilder.RenameTable(
                name: "SampleSamplePlaylists",
                newName: "SampleSamplePlaylist");

            migrationBuilder.RenameTable(
                name: "SamplePackSamplePackGenres",
                newName: "SamplePackSamplePackGenre");

            migrationBuilder.RenameIndex(
                name: "IX_SampleSamplePlaylists_SamplePlaylistId",
                table: "SampleSamplePlaylist",
                newName: "IX_SampleSamplePlaylist_SamplePlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_SamplePackSamplePackGenres_SamplePackGenreId",
                table: "SamplePackSamplePackGenre",
                newName: "IX_SamplePackSamplePackGenre_SamplePackGenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SampleSamplePlaylist",
                table: "SampleSamplePlaylist",
                columns: new[] { "SampleId", "SamplePlaylistId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamplePackSamplePackGenre",
                table: "SamplePackSamplePackGenre",
                columns: new[] { "SamplePackId", "SamplePackGenreId" });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FollowerId = table.Column<string>(type: "text", nullable: false),
                    FolloweeId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => new { x.FollowerId, x.FolloweeId });
                    table.ForeignKey(
                        name: "FK_Follow_User_FolloweeId",
                        column: x => x.FolloweeId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follow_User_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SampleLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SampleLike_Sample_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Sample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SamplePackLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SamplePackId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePackLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplePackLike_SamplePack_SamplePackId",
                        column: x => x.SamplePackId,
                        principalTable: "SamplePack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SamplePackLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylistGenre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GenreName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePlaylistGenre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylistLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SamplePackId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePlaylistLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplePlaylistLike_SamplePack_SamplePackId",
                        column: x => x.SamplePackId,
                        principalTable: "SamplePack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SamplePlaylistLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SampleTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SampleTagName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylistSamplePlaylistGenre",
                columns: table => new
                {
                    SamplePlaylistId = table.Column<Guid>(type: "uuid", nullable: false),
                    SamplePlaylistGenreId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePlaylistSamplePlaylistGenre", x => new { x.SamplePlaylistId, x.SamplePlaylistGenreId });
                    table.ForeignKey(
                        name: "FK_SamplePlaylistSamplePlaylistGenre_SamplePlaylistGenre_Sampl~",
                        column: x => x.SamplePlaylistGenreId,
                        principalTable: "SamplePlaylistGenre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SamplePlaylistSamplePlaylistGenre_SamplePlaylist_SamplePlay~",
                        column: x => x.SamplePlaylistId,
                        principalTable: "SamplePlaylist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleSampleTag",
                columns: table => new
                {
                    SampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    SampleTagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleSampleTag", x => new { x.SampleId, x.SampleTagId });
                    table.ForeignKey(
                        name: "FK_SampleSampleTag_Sample_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Sample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleSampleTag_SampleTag_SampleTagId",
                        column: x => x.SampleTagId,
                        principalTable: "SampleTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FolloweeId",
                table: "Follow",
                column: "FolloweeId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleLike_SampleId",
                table: "SampleLike",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleLike_UserId",
                table: "SampleLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePackLike_SamplePackId",
                table: "SamplePackLike",
                column: "SamplePackId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePackLike_UserId",
                table: "SamplePackLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePlaylistLike_SamplePackId",
                table: "SamplePlaylistLike",
                column: "SamplePackId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePlaylistLike_UserId",
                table: "SamplePlaylistLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePlaylistSamplePlaylistGenre_SamplePlaylistGenreId",
                table: "SamplePlaylistSamplePlaylistGenre",
                column: "SamplePlaylistGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleSampleTag_SampleTagId",
                table: "SampleSampleTag",
                column: "SampleTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_SamplePackSamplePackGenre_SamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenre",
                column: "SamplePackGenreId",
                principalTable: "SamplePackGenre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SamplePackSamplePackGenre_SamplePack_SamplePackId",
                table: "SamplePackSamplePackGenre",
                column: "SamplePackId",
                principalTable: "SamplePack",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleSamplePlaylist_Sample_SampleId",
                table: "SampleSamplePlaylist",
                column: "SampleId",
                principalTable: "Sample",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleSamplePlaylist_SamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylist",
                column: "SamplePlaylistId",
                principalTable: "SamplePlaylist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
