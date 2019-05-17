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

        public async Task CreateOrUpdate(CreateReadRequest request)
        {
            var existingReadTask = _context.Read.FindAsync(request.Id);

            var goodreadsBookTask = _goodreadsService.GetBookDetails(request.GoodreadsId);

            Task<GoodreadsBookResponse> goodreadsEditionBookTask = null;
            if (request.GoodreadsEditionId.HasValue)
            {
                goodreadsEditionBookTask = _goodreadsService.GetBookDetails(request.GoodreadsEditionId.Value);
            }

            await Task.WhenAll(new Task[] { goodreadsBookTask, goodreadsEditionBookTask, existingReadTask }.Where(_ => _ != null));
            var goodreadsBook = goodreadsBookTask.Result;
            var goodreadsEditionBook = goodreadsEditionBookTask?.Result;
            var existingRead = existingReadTask.Result;

            if (existingRead == null)
            {
                var read = new Read
                {
                    CoverPath = goodreadsEditionBook != null ? goodreadsEditionBook.CoverPath : goodreadsBook.CoverPath,
                    Description = goodreadsBook.Description,
                    Title = goodreadsBook.Title,
                    GoodreadsId = goodreadsBook.Id,
                    GoodreadsEditionId = goodreadsEditionBook?.Id,
                    Rating = request.Rating,
                    ReleaseDate = goodreadsBook.ReleaseDate
                };

                _context.Read.Add(read);
            }
            else
            {
                existingRead.CoverPath = goodreadsEditionBook != null ? goodreadsEditionBook.CoverPath : goodreadsBook.CoverPath;
                existingRead.Description = goodreadsBook.Description;
                existingRead.Title = goodreadsBook.Title;
                existingRead.GoodreadsEditionId = goodreadsEditionBook?.Id;
                existingRead.Rating = request.Rating;
                existingRead.ReleaseDate = goodreadsBook.ReleaseDate;

                _context.Update(existingRead);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var read = await _context.Read.FindAsync(id);
            _context.Read.Remove(read);
            await _context.SaveChangesAsync();
        }
    }
}
