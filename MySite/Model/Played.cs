namespace MySite.Model
{
    public class Played
    {
        public int Id { get; set; }
        public string CoverPath { get; set; }
        public string Description { get; set; }
        public int IdgbId { get; set; }
        public Rating Rating { get; set; }
        public string Title { get; set; }
    }
}
