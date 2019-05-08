using System;
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

        public async Task Create(CreateWatchedRequest request)
        {
            var tmdbMovie = await _tmdbService.GetMovieDetails(request.TmdbId);

            var watched = new Watched
            {
                PosterPath = tmdbMovie.Poster,
                Title = tmdbMovie.Title,
                TmdbId = tmdbMovie.Id,
                Rating = request.Rating,
                ReleaseDate = tmdbMovie.ReleaseDate
            };

            _context.Watched.Add(watched);
            await _context.SaveChangesAsync();
        }
    }
}
