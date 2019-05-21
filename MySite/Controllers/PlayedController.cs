using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySite.Model;
using MySite.Models;
using MySite.Services;

namespace MySite.Controllers
{
    public class PlayedController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IgdbService _igdbService;
        private readonly PlayedService _playedService;

        public PlayedController(ApplicationContext context, IOptions<Config> config)
        {
            _context = context;
            _igdbService = new IgdbService(config.Value);
            _playedService = new PlayedService(_context, _igdbService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string searchQuery)
        {
            var results = await _igdbService.SearchAsync(searchQuery);

            var viewModel = new SelectPlayedViewModel(results.ToList());
            return PartialView("Select", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Select(int igdbId)
        {
            var igdbGameResponse = await _igdbService.GetDetails(igdbId);
            var viewModel = new CreatePlayedViewModel(igdbGameResponse);

            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectForUpdate(int playedId)
        {
            var played = await _context.Played.FindAsync(playedId);
            var igdbGameResponse = await _igdbService.GetDetails(played.IdgbId);

            var viewModel = new CreatePlayedViewModel(played, igdbGameResponse);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(CreatePlayedViewModel viewModel)
        {
            await _playedService.CreateOrUpdate(viewModel.Request);
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int readId)
        {
            await _playedService.Delete(readId);
            return Ok();
        }
    }

    public class PlayedListViewComponent : ViewComponent
    {
        const int ListPageSize = 20;
        private readonly ApplicationContext _context;

        public PlayedListViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page)
        {
            var playedList = await _context.Played.OrderByDescending(_ => _.Id).Skip(ListPageSize * (page - 1)).Take(ListPageSize).ToListAsync();
            var viewModel = new List<ListPlayedViewModel>();
            foreach (var played in playedList)
            {
                viewModel.Add(new ListPlayedViewModel(played));
            }

            return View("~/Views/Played/List.cshtml", viewModel);
        }
    }
}
