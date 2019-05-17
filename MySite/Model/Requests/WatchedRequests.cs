namespace MySite.Model.Requests
{
    public class CreateWatchedRequest
    {
        public int? Id { get; set; }
        public Rating Rating { get; set; }
        public int? SeasonNumber { get; set; }
        public int TmdbId { get; set; }
        public WatchedType WatchedType { get; set; }
    }
}
