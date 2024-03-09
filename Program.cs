using TunaPiano.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

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


// Artist Endpoints



// Genre Endpoints



app.Run();
