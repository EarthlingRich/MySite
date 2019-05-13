using System;
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
    public class WatchedController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly TmdbService _tmdbService;
        private readonly WacthedService _watchedService;

        public WatchedController(ApplicationContext context, IOptions<Config> config)
        {
            _context = context;
            _tmdbService = new TmdbService(config.Value);
            _watchedService = new WacthedService(_context, _tmdbService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string searchQuery)
        {
            var results = await _tmdbService.SearchAsync(searchQuery);

            var viewModel = new SelectWatchedViewModel(results.ToList());
            return PartialView("Select", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Select(int tmdbId)
        {
            var tmdbMovie = await _tmdbService.GetMovieDetails(tmdbId);

            var viewModel = new CreateWatchedViewModel(tmdbMovie);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectForUpdate(int watchedId)
        {
            var watched = await _context.Watched.FindAsync(watchedId);
            var viewModel = new CreateWatchedViewModel(watched);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(CreateWatchedViewModel viewModel)
        {
            await _watchedService.CreateOrUpdate(viewModel.Request);
            return Redirect("Index");
        }
    }

    public class WatchedListViewComponent : ViewComponent
    {
        const int ListPageSize = 20;
        private readonly ApplicationContext _context;

        public WatchedListViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page)
        {
            var watched = await _context.Watched.OrderByDescending(_ => _.Id).Skip(ListPageSize * (page - 1)).Take(ListPageSize).ToListAsync();
            var viewModel = new List<ListWatchedViewModel>();
            foreach(var watch in watched)
            {
                viewModel.Add(new ListWatchedViewModel(watch));
            }

            return View("~/Views/Watched/List.cshtml", viewModel);
        }
    }
}
