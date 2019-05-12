namespace MySite.Model.Requests
{
    public class CreateReadRequest
    {
        public int GoodreadsId { get; set; }
        public int? GoodreadsEditionId { get; set; }
        public Rating Rating { get; set; }
    }
}
