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
            var tmdbTask = _tmdbService.GetDetails(request.TmdbId, request.WatchedType);

            await Task.WhenAll(existingWatchedTask, tmdbTask);

            var existingWatched = existingWatchedTask.Result;
            var tmdbMovie = tmdbTask.Result;

            if (existingWatched == null)
            {
                var watched = new Watched
                {
                    PosterPath = tmdbMovie.PosterPath,
                    Description = tmdbMovie.Overview,
                    Title = tmdbMovie.Title,
                    TmdbId = tmdbMovie.Id,
                    Rating = request.Rating,
                    ReleaseDate = tmdbMovie.ReleaseDate,
                    WatchedType = request.WatchedType
                };

                _context.Watched.Add(watched);
            }
            else
            {
                existingWatched.PosterPath = tmdbMovie.PosterPath;
                existingWatched.Description = tmdbMovie.Overview;
                existingWatched.Title = tmdbMovie.Title;
                existingWatched.Rating = request.Rating;
                existingWatched.ReleaseDate = tmdbMovie.ReleaseDate;

                _context.Watched.Update(existingWatched);
            }

            await _context.SaveChangesAsync();
        }
    }
}
