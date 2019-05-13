namespace MySite.Model.Requests
{
    public class CreateWatchedRequest
    {
        public int? Id { get; set; }
        public Rating Rating { get; set; }
        public int TmdbId { get; set; }
    }
}
