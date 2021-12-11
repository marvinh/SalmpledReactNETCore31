using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Salmpled.Migrations
{
    public partial class Username : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SamplePackGenre",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    GenreName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePackGenre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylistGenre",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    GenreName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePlaylistGenre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SampleTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SampleTagName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(maxLength: 64, nullable: true),
                    AuthProvider = table.Column<string>(nullable: true),
                    Headline = table.Column<string>(maxLength: 128, nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    UserImageRegion = table.Column<string>(nullable: true),
                    UserImageBucket = table.Column<string>(nullable: true),
                    UserImageKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FollowerId = table.Column<string>(nullable: false),
                    FolloweeId = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
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
                name: "SamplePack",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SamplePackName = table.Column<string>(maxLength: 128, nullable: true),
                    SamplePackImageRegion = table.Column<string>(nullable: true),
                    SamplePackImageBucket = table.Column<string>(nullable: true),
                    SamplePackImageKey = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Published = table.Column<bool>(nullable: false),
                    PublishedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplePack_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylist",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SamplePlaylistName = table.Column<string>(maxLength: 128, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    SamplePlaylistImageRegion = table.Column<string>(nullable: true),
                    SamplePlaylistImageBucket = table.Column<string>(nullable: true),
                    SamplePlaylistImageKey = table.Column<string>(nullable: true),
                    Published = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePlaylist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplePlaylist_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrginalFileName = table.Column<string>(nullable: true),
                    RenamedFileName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Bucket = table.Column<string>(nullable: true),
                    UncompressedSampleKey = table.Column<string>(nullable: true),
                    CompressedSampleKey = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SamplePackId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sample_SamplePack_SamplePackId",
                        column: x => x.SamplePackId,
                        principalTable: "SamplePack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sample_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SamplePackLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SamplePackId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
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
                name: "SamplePackSamplePackGenre",
                columns: table => new
                {
                    SamplePackId = table.Column<Guid>(nullable: false),
                    SamplePackGenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplePackSamplePackGenre", x => new { x.SamplePackId, x.SamplePackGenreId });
                    table.ForeignKey(
                        name: "FK_SamplePackSamplePackGenre_SamplePackGenre_SamplePackGenreId",
                        column: x => x.SamplePackGenreId,
                        principalTable: "SamplePackGenre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SamplePackSamplePackGenre_SamplePack_SamplePackId",
                        column: x => x.SamplePackId,
                        principalTable: "SamplePack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SamplePlaylistLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SamplePackId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
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
                name: "SamplePlaylistSamplePlaylistGenre",
                columns: table => new
                {
                    SamplePlaylistId = table.Column<Guid>(nullable: false),
                    SamplePlaylistGenreId = table.Column<Guid>(nullable: false)
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
                name: "SampleLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SampleId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
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
                name: "SampleSamplePlaylist",
                columns: table => new
                {
                    SampleId = table.Column<Guid>(nullable: false),
                    SamplePlaylistId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleSamplePlaylist", x => new { x.SampleId, x.SamplePlaylistId });
                    table.ForeignKey(
                        name: "FK_SampleSamplePlaylist_Sample_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Sample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleSamplePlaylist_SamplePlaylist_SamplePlaylistId",
                        column: x => x.SamplePlaylistId,
                        principalTable: "SamplePlaylist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleSampleTag",
                columns: table => new
                {
                    SampleId = table.Column<Guid>(nullable: false),
                    SampleTagId = table.Column<Guid>(nullable: false)
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
                name: "IX_Sample_SamplePackId",
                table: "Sample",
                column: "SamplePackId");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_UserId",
                table: "Sample",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleLike_SampleId",
                table: "SampleLike",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleLike_UserId",
                table: "SampleLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePack_UserId",
                table: "SamplePack",
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
                name: "IX_SamplePackSamplePackGenre_SamplePackGenreId",
                table: "SamplePackSamplePackGenre",
                column: "SamplePackGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplePlaylist_UserId",
                table: "SamplePlaylist",
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
                name: "IX_SampleSamplePlaylist_SamplePlaylistId",
                table: "SampleSamplePlaylist",
                column: "SamplePlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleSampleTag_SampleTagId",
                table: "SampleSampleTag",
                column: "SampleTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "SampleLike");

            migrationBuilder.DropTable(
                name: "SamplePackLike");

            migrationBuilder.DropTable(
                name: "SamplePackSamplePackGenre");

            migrationBuilder.DropTable(
                name: "SamplePlaylistLike");

            migrationBuilder.DropTable(
                name: "SamplePlaylistSamplePlaylistGenre");

            migrationBuilder.DropTable(
                name: "SampleSamplePlaylist");

            migrationBuilder.DropTable(
                name: "SampleSampleTag");

            migrationBuilder.DropTable(
                name: "SamplePackGenre");

            migrationBuilder.DropTable(
                name: "SamplePlaylistGenre");

            migrationBuilder.DropTable(
                name: "SamplePlaylist");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "SampleTag");

            migrationBuilder.DropTable(
                name: "SamplePack");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
