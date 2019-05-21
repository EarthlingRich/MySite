using System.Threading.Tasks;
using MySite.Model;
using MySite.Model.Requests;

namespace MySite.Services
{
    public class PlayedService
    {
        private readonly ApplicationContext _context;
        private readonly IgdbService _igdbService;

        public PlayedService(ApplicationContext context, IgdbService igdbService)
        {
            _context = context;
            _igdbService = igdbService;
        }

        public async Task CreateOrUpdate(CreatePlayedRequest request)
        {
            var existingPlayedTask = _context.Played.FindAsync(request.Id);
            var igdbGameTask = _igdbService.GetDetails(request.IgdbId);

            await Task.WhenAll(existingPlayedTask, igdbGameTask);
            var existingPlayed = existingPlayedTask.Result;
            var igdbGame = igdbGameTask.Result;

            if (existingPlayed == null)
            {
                var played = new Played
                {
                    CoverPath = igdbGame.Cover != null ? igdbGame.Cover.Url : string.Empty,
                    Description = igdbGame.Description,
                    Title = igdbGame.Title,
                    IdgbId = igdbGame.Id,
                    Rating = request.Rating,
                };

                _context.Played.Add(played);
            }
            else
            {
                existingPlayed.CoverPath = igdbGame.Cover != null ? igdbGame.Cover.Url : string.Empty;
                existingPlayed.Description = igdbGame.Description;
                existingPlayed.Title = igdbGame.Title;
                existingPlayed.Rating = request.Rating;

                _context.Update(existingPlayed);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var played = await _context.Played.FindAsync(id);
            _context.Played.Remove(played);
            await _context.SaveChangesAsync();
        }
    }
}
