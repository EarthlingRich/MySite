namespace MySite.Model.Requests
{
    public class CreateReadRequest
    {
        public int? Id { get; set; }
        public int GoodreadsId { get; set; }
        public int? GoodreadsEditionId { get; set; }
        public Rating Rating { get; set; }
    }
}
