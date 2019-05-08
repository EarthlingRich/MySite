namespace MySite.Model.Requests
{
    public class CreateWatchedRequest
    {
        public Rating Rating { get; set; }
        public int TmdbId { get; set; }
    }
}
