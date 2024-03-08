﻿namespace TunaPiano.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public ICollection<Genre>? Genres { get; set; }
    }
}
