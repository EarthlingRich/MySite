using System;

namespace MySite.Model
{
    public class Watched
    {
        public int Id { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public int TmdbId { get; set; }
        public Rating Rating { get; set; }
    }
}
