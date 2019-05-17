using System.Linq;
using System.Threading.Tasks;
using MySite.Model;
using MySite.Model.Requests;

namespace MySite.Services
{
    public class WacthedService
    {
        private readonly ApplicationContext _context;
        private readonly TmdbService _tmdbService;

        public WacthedService(ApplicationContext context, TmdbService tmdbService)
        {
            _context = context;
            _tmdbService = tmdbService;
        }

        public async Task CreateOrUpdate(CreateWatchedRequest request)
        {
            var existingWatchedTask = _context.Watched.FindAsync(request.Id);
            var tmdbTask = _tmdbService.GetDetails(request.TmdbId, request.SeasonNumber, request.WatchedType);

            await Task.WhenAll(existingWatchedTask, tmdbTask);

            var existingWatched = existingWatchedTask.Result;
            var tmdbWatchedResponse = tmdbTask.Result;

            if (existingWatched == null)
            {
                var watched = new Watched
                {
                    PosterPath = tmdbWatchedResponse.PosterPath,
                    Description = tmdbWatchedResponse.Overview,
                    Title = tmdbWatchedResponse.Title,
                    TmdbId = tmdbWatchedResponse.Id,
                    SeasonNumber = request.SeasonNumber,
                    Rating = request.Rating,
                    ReleaseDate = tmdbWatchedResponse.ReleaseDate,
                    WatchedType = request.WatchedType
                };
                _context.Watched.Add(watched);
            }
            else
            {
                existingWatched.PosterPath = tmdbWatchedResponse.PosterPath;
                existingWatched.Description = tmdbWatchedResponse.Overview;
                existingWatched.Title = tmdbWatchedResponse.Title;
                existingWatched.Rating = request.Rating;
                existingWatched.ReleaseDate = tmdbWatchedResponse.ReleaseDate;

                _context.Watched.Update(existingWatched);
            }

            await _context.SaveChangesAsync();
        }
    }
}
