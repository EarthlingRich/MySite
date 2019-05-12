using System.Linq;
using System.Threading.Tasks;
using MySite.Model;
using MySite.Model.Requests;

namespace MySite.Services
{
    public class ReadService
    {
        private readonly ApplicationContext _context;
        private readonly GoodreadsService _goodreadsService;

        public ReadService(ApplicationContext context, GoodreadsService goodreadsService)
        {
            _context = context;
            _goodreadsService = goodreadsService;
        }

        public async Task Create(CreateReadRequest request)
        {
            var goodreadsBookTask = _goodreadsService.GetBookDetails(request.GoodreadsId);

            Task<GoodreadsBookResponse> goodreadsEditionBookTask = null;
            if (request.GoodreadsEditionId.HasValue)
            {
                goodreadsEditionBookTask = _goodreadsService.GetBookDetails(request.GoodreadsEditionId.Value);
            }

            await Task.WhenAll(new Task[] { goodreadsBookTask, goodreadsEditionBookTask }.Where(_ => _ != null));
            var goodreadsBook = goodreadsBookTask.Result;
            var goodreadsEditionBook = goodreadsEditionBookTask?.Result;

            var read = new Read
            {
                CoverPath = goodreadsEditionBook != null ? goodreadsEditionBook.CoverPath : goodreadsBook.CoverPath,
                Title = goodreadsBook.Title,
                GoodreadsId = goodreadsBook.Id,
                GoodreadsEditionId = goodreadsEditionBook?.Id,
                Rating = request.Rating,
                ReleaseDate = goodreadsBook.ReleaseDate
            };

            _context.Read.Add(read);
            await _context.SaveChangesAsync();
        }
    }
}
