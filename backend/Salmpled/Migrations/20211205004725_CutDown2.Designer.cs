// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Salmpled.Models;

namespace Salmpled.Migrations
{
    [DbContext(typeof(SalmpledContext))]
    [Migration("20211205004725_CutDown2")]
    partial class CutDown2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Salmpled.Models.Sample", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bucket")
                        .HasColumnType("text");

                    b.Property<string>("CompressedSampleKey")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("OrginalFileName")
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("RenamedFileName")
                        .HasColumnType("text");

                    b.Property<Guid>("SamplePackId")
                        .HasColumnType("uuid");

                    b.Property<string>("UncompressedSampleKey")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SamplePackId");

                    b.HasIndex("UserId");

                    b.ToTable("Sample");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SamplePackImageBucket")
                        .HasColumnType("text");

                    b.Property<string>("SamplePackImageKey")
                        .HasColumnType("text");

                    b.Property<string>("SamplePackImageRegion")
                        .HasColumnType("text");

                    b.Property<string>("SamplePackName")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SamplePack");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePackGenre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("GenreName")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("SamplePackGenre");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePackSamplePackGenre", b =>
                {
                    b.Property<Guid>("SamplePackId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SamplePackGenreId")
                        .HasColumnType("uuid");

                    b.HasKey("SamplePackId", "SamplePackGenreId");

                    b.HasIndex("SamplePackGenreId");

                    b.ToTable("SamplePackSamplePackGenres");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePlaylist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<string>("SamplePlaylistImageBucket")
                        .HasColumnType("text");

                    b.Property<string>("SamplePlaylistImageKey")
                        .HasColumnType("text");

                    b.Property<string>("SamplePlaylistImageRegion")
                        .HasColumnType("text");

                    b.Property<string>("SamplePlaylistName")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SamplePlaylist");
                });

            modelBuilder.Entity("Salmpled.Models.SampleSamplePlaylist", b =>
                {
                    b.Property<Guid>("SampleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SamplePlaylistId")
                        .HasColumnType("uuid");

                    b.HasKey("SampleId", "SamplePlaylistId");

                    b.HasIndex("SamplePlaylistId");

                    b.ToTable("SampleSamplePlaylists");
                });

            modelBuilder.Entity("Salmpled.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthProvider")
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Headline")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserImageBucket")
                        .HasColumnType("text");

                    b.Property<string>("UserImageKey")
                        .HasColumnType("text");

                    b.Property<string>("UserImageRegion")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Salmpled.Models.Sample", b =>
                {
                    b.HasOne("Salmpled.Models.SamplePack", "SamplePack")
                        .WithMany("Samples")
                        .HasForeignKey("SamplePackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salmpled.Models.User", "User")
                        .WithMany("Samples")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePack", b =>
                {
                    b.HasOne("Salmpled.Models.User", "User")
                        .WithMany("SamplePacks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Salmpled.Models.SamplePackSamplePackGenre", b =>
                {
                    b.HasOne("Salmpled.Models.SamplePackGenre", "SamplePackGenre")
                        .WithMany("SamplePackSamplePackGenres")
                        .HasForeignKey("SamplePackGenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salmpled.Models.SamplePack", "SamplePack")
                        .WithMany("SamplePackSamplePackGenres")
                        .HasForeignKey("SamplePackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Salmpled.Models.SamplePlaylist", b =>
                {
                    b.HasOne("Salmpled.Models.User", "User")
                        .WithMany("SamplePlaylists")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Salmpled.Models.SampleSamplePlaylist", b =>
                {
                    b.HasOne("Salmpled.Models.Sample", "Sample")
                        .WithMany("SampleSamplePlaylists")
                        .HasForeignKey("SampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salmpled.Models.SamplePlaylist", "SamplePlaylist")
                        .WithMany("SampleSamplePlaylists")
                        .HasForeignKey("SamplePlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
