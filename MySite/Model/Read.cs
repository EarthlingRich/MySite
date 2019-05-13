using System;

namespace MySite.Model
{
    public class Read
    {
        public int Id { get; set; }
        public string CoverPath { get; set; }
        public string Description { get; set; }
        public int GoodreadsId { get; set; }
        public int? GoodreadsEditionId { get; set; }
        public Rating Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
    }
}
