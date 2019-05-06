using System;
using System.Threading.Tasks;
using MySite.Model;
using MySite.Model.Requests;

namespace MySite.Services
{
    public class MovieService
    {
        private readonly ApplicationContext _context;
        private readonly TmdbService _tmdbService;

        public MovieService(ApplicationContext context, TmdbService tmdbService)
        {
            _context = context;
            _tmdbService = tmdbService;
        }

        public async Task AddMovie(AddMovieRequest request)
        {
            var tmdbMovie = await _tmdbService.GetMovieDetails(request.TmdbId);

            var movie = new Movie
            {
                Title = tmdbMovie.Title,
                TmdbId = tmdbMovie.Id
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
    }
}
