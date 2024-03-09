using Microsoft.EntityFrameworkCore;
namespace TunaPiano.Models;

public class TunaPianoDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public DbSet<SongGenre> SongGenres { get; set; }

    public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Configure composite primary key for SongGenre
        modelBuilder.Entity<SongGenre>()
            .HasKey(sg => new { sg.SongId, sg.GenreId });

        // Seed data for Artists
        modelBuilder.Entity<Artist>().HasData(new Artist[]
        {
        new Artist {Id = 1, Name="Alison Krauss", Age=51, Bio="An American bluegrass-country singer and musician."},
        new Artist {Id = 2, Name="Chris Thile", Age=40, Bio="An American mandolinist, singer, songwriter, composer, and radio personality, best known for his work in the progressive acoustic trio Nickel Creek and the acoustic folk/bluegrass quintet Punch Brothers."},
        });

        // Seed data for Songs
        modelBuilder.Entity<Song>().HasData(new Song[]
        {
        new Song {Id = 1, Album="Forget About It", ArtistId=1, Title="Stay", Length=248},
        new Song {Id = 2, Album="Thanks for Listening", ArtistId=2, Title="Thank You, New York", Length=194},
        });

        // Seed data for Genres
        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
        new Genre {Id = 1, Description="Folk"},
        new Genre {Id = 2, Description="Acoustic"},
        new Genre {Id = 3, Description="Americana"},
        new Genre {Id = 4, Description="Indie"},
        new Genre {Id = 5, Description="Alternative Country"},
        });
    }


}