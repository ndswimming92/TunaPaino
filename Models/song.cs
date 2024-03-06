﻿using System.ComponentModel.DataAnnotations;

namespace TunaPaino.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int ArtistId { get; set; }
        public string? Album { get; set;}
        public int Length { get; set; }

        public ICollection<Song>? Songs { get; set; }
    }
}
