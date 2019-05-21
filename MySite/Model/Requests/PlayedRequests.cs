namespace MySite.Model.Requests
{
    public class CreatePlayedRequest
    {
        public int? Id { get; set; }
        public int IgdbId { get; set; }
        public Rating Rating { get; set; }
    }
}
