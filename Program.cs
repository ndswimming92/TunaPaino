using TunaPiano.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// ENDPOINTS--------------------------------------------------------------->

// Song Endpoints
// Create a Song
app.MapPost("/api/createNewSong", (TunaPianoDbContext db, Song song) =>
{
    db.Songs.Add(song);
    db.SaveChanges();

    return Results.Created($"/api/createNewSongsong.Id", song);
});

// View list of all Songs
app.MapGet("/api/viewAllSongs", (TunaPianoDbContext db) =>
{
    return db.Songs.ToList();
});

// Update a Song
app.MapPut("/api/updateSong/{songId}", (TunaPianoDbContext db, Song song, int id) =>
{
    Song updatedSong = db.Songs.SingleOrDefault(s => s.Id == id);
    if (updatedSong == null)
    {
        return Results.NotFound();
    }
    updatedSong.Title = song.Title;
    updatedSong.ArtistId = song.ArtistId;
    updatedSong.Album = song.Album;
    updatedSong.Length = song.Length;

    db.SaveChanges();
    return Results.Created($"/api/updateSong/song.id", song);

});

// Delete a song
app.MapDelete("/api/deleteSong/{songId}", (TunaPianoDbContext db, int id) =>
{
    var song = db.Songs.SingleOrDefault(s => s.Id == id);

    if (song == null)
    {
        return Results.NotFound("Song not found.");
    }
    db.Songs.Remove(song);
    db.SaveChanges();

    return Results.Ok("Song deleted successfully.");
});

// Details view of a single Song and its associated genres and artist details

/*
 * app.MapGet("/api/detailsViewofSongs/{songId}", (TunaPianoDbContext db, int id) =>
{

    var song = db.Songs.Include(s => s.ArtistId).Include(s => s.Genres).FirstOrDefault(s => s.Id == id);
    if (song == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(song);

});

// Asigning Genre to Song
app.MapPost("/api/assignGenreToSong", (TunaPianoDbContext db, int songId, int genreId) =>
{
    var song = db.Songs.SingleOrDefault(s => s.Id == songId);
    var genre = db.Genres.SingleOrDefault(g => g.Id == genreId);

    if (song.Genres == null)
    {
        song.Genres = new List<Genre>();
    }
    song.Genres.Add(genre);
    db.SaveChanges();
    return song;

});
*/

// Artist Endpoints
// Create a Artist
app.MapPost("/api/createNewArtist", (TunaPianoDbContext db, Artist artist) =>
{
    db.Artists.Add(artist);
    db.SaveChanges();
    return Results.Created($"/api/createNewArtist/artist.Id", artist);
});


// View list of all Artists
app.MapGet("/api/viewAllArtists", (TunaPianoDbContext db) =>
{
    return db.Artists.ToList();

});


// Update a Artist
app.MapPut("/api/updateArtist/{artistId}", (TunaPianoDbContext db, Artist artist, int id) =>
{
    Artist updatedArtist = db.Artists.SingleOrDefault(a => a.Id == id);
    if (updatedArtist == null)
    {
        return Results.NotFound();
    }
    updatedArtist.Name = artist.Name;
    updatedArtist.Age = artist.Age;
    updatedArtist.Bio = artist.Bio;

    db.SaveChanges();
    return Results.Created($"/api/updateArtist/artist.id", artist);

});


// Delete a Artist
app.MapDelete("/api/deleteArtist/{artistId}", (TunaPianoDbContext db, int id) =>
{
    var artist = db.Artists.SingleOrDefault(a => a.Id == id);
    if (artist == null)
    {
        return Results.NotFound("Artist not found.");
    }
    db.Artists.Remove(artist);
    db.SaveChanges();
    return Results.NoContent();

});


// Genre Endpoints
// Create a Genre
app.MapPost("/api/createNewGenre", (TunaPianoDbContext db, Genre genre) =>
{
    db.Genres.Add(genre);
    db.SaveChanges();
    return Results.Created($"/api/createNewGenre/genre.id", genre);
});


// View list of all Genres
app.MapGet("/api/viewAllGenres", (TunaPianoDbContext db) =>
{
    return db.Genres.ToList();
});


// Update Genre
app.MapPut("/api/updateGenre/{genreId}", (TunaPianoDbContext db, Genre genre, int id) =>
{
    Genre updatedGenre = db.Genres.SingleOrDefault(gen => gen.Id == id);
    if (updatedGenre == null)
    {
        return Results.NotFound("Genre not found");
    }
    updatedGenre.Description = genre.Description;
    db.SaveChanges();
    return Results.Created($"/api/updateGenre/genre.id", genre);
});


// Delete a Genre
app.MapDelete("/api/deleteGenre/{genreId}", (TunaPianoDbContext db, int id) =>
{
    var genre = db.Genres.SingleOrDefault(g => g.Id == id);
    if (genre == null)
    {
        return Results.NotFound("Genre not found.");
    }
    db.Genres.Remove(genre);
    db.SaveChanges();
    return Results.NoContent();
});

// View Single Genre and Associated Songs
app.MapGet("/api/genreAssociatedSongs/{genreId}", (TunaPianoDbContext db, int genreId) =>

{
    var genreWithSongs = db.Genres
        .Include(g => g.Songs)
        .FirstOrDefault(gs => gs.Id == genreId);
    return genreWithSongs;

});

app.Run();
