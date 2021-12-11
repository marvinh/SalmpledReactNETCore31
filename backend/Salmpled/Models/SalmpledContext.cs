using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace Salmpled.Models
{
    public class SalmpledContext : DbContext
    {
        public SalmpledContext(DbContextOptions<SalmpledContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(connectionUrl);
            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
            string connectionString = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
            optionsBuilder.UseNpgsql(connectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SampleSamplePlaylist>()
            .HasKey(ssp => new { ssp.SampleId, ssp.SamplePlaylistId });

            modelBuilder.Entity<SamplePackSamplePackGenre>()
            .HasKey(spspg => new {spspg.SamplePackId, spspg.SamplePackGenreId});
            //TODO FOR LATER 

            // modelBuilder.Entity<SampleSampleTag>()
            //     .HasKey(sst => new { sst.SampleId, sst.SampleTagId });
            // modelBuilder.Entity<SampleSampleTag>()
            //     .HasOne(sst => sst.Sample)
            //     .WithMany(s => s.SampleSampleTags)
            //     .HasForeignKey(sst => sst.SampleId);
            // modelBuilder.Entity<SampleSampleTag>()
            //     .HasOne(sst => sst.SampleTag)
            //     .WithMany(st => st.SampleSampleTags)
            //     .HasForeignKey(sst => sst.SampleTagId);


            // modelBuilder.Entity<SampleSamplePlaylist>()
            //     .HasKey(ssp => new { ssp.SampleId, ssp.SamplePlaylistId });

            // modelBuilder.Entity<SampleSamplePlaylist>()
            //     .HasOne(ssp => ssp.Sample)
            //     .WithMany(s => s.SampleSamplePlaylists)
            //     .HasForeignKey(ssp => ssp.SampleId);

    //         modelBuilder.Entity<SampleSamplePlaylist>()
    //             .HasOne(ssp => ssp.SamplePlaylist)
    //             .WithMany(sp => sp.SampleSamplePlaylists)
    //             .HasForeignKey(ssp => ssp.SamplePlaylistId);

    //         modelBuilder.Entity<SamplePackSamplePackGenre>()
    //             .HasKey(spspg => new { spspg.SamplePackId, spspg.SamplePackGenreId });

    //         modelBuilder.Entity<SamplePackSamplePackGenre>()
    //             .HasOne(spspg => spspg.SamplePack)
    //             .WithMany(sp => sp.SamplePackSamplePackGenres)
    //             .OnDelete(DeleteBehavior.Cascade)
    //             .HasForeignKey(spspg => spspg.SamplePackId);

    //         modelBuilder.Entity<SamplePlaylistSamplePlaylistGenre>()
    //             .HasOne(spspg => spspg.SamplePlaylistGenre)
    //             .WithMany(spg => spg.SamplePlaylistSamplePlaylistGenres)
    //             .HasForeignKey(spspg => spspg.SamplePlaylistGenreId);

    //         modelBuilder.Entity<SamplePlaylistSamplePlaylistGenre>()
    // .HasKey(spspg => new { spspg.SamplePlaylistId, spspg.SamplePlaylistGenreId });

    //         modelBuilder.Entity<SamplePlaylistSamplePlaylistGenre>()
    //             .HasOne(spspg => spspg.SamplePlaylist)
    //             .WithMany(sp => sp.SamplePlaylistSamplePlaylistGenres)
    //             .HasForeignKey(spspg => spspg.SamplePlaylistId);

    //         modelBuilder.Entity<SamplePlaylistSamplePlaylistGenre>()
    //             .HasOne(spspg => spspg.SamplePlaylistGenre)
    //             .WithMany(spg => spg.SamplePlaylistSamplePlaylistGenres)
    //             .HasForeignKey(spspg => spspg.SamplePlaylistGenreId);

    //         modelBuilder.Entity<Follow>()
    // .HasKey(k => new { k.FollowerId, k.FolloweeId });
    //         modelBuilder.Entity<Follow>()
    //             .HasOne(u => u.Followee)
    //             .WithMany(u => u.Follower)
    //             .HasForeignKey(u => u.FollowerId)
    //             .OnDelete(DeleteBehavior.Restrict);
    //         modelBuilder.Entity<Follow>()
    //             .HasOne(u => u.Follower)
    //             .WithMany(u => u.Followee)
    //             .HasForeignKey(u => u.FolloweeId)
    //             .OnDelete(DeleteBehavior.Restrict);


        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is TimestampBaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((TimestampBaseEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((TimestampBaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Sample> Sample { get; set; }
        public DbSet<SamplePack> SamplePack { get; set; }
       
        public DbSet<SamplePackGenre> SamplePackGenre { get; set; }

        public DbSet<SamplePackSamplePackGenre> SamplePackSamplePackGenres {get;set;}

        public DbSet<SamplePlaylist> SamplePlaylist { get; set; }

        //MANY TO MANY
        public DbSet<SampleSamplePlaylist> SampleSamplePlaylists{get; set;}

        //TODO FOR LATER

        // public DbSet<SamplePackLike> SamplePackLike { get; set; }

        // public DbSet<SampleLike> SampleLike { get; set; }

        //public DbSet<SamplePlaylistLike> SamplePlaylistLike { get; set; }

        //public DbSet<SampleTag> SampleTag { get; set; }

        //public DbSet<Follow> Follow { get; set; }


    }

}