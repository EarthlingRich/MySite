using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySite.Model;
using MySite.Models;
using MySite.Services;

namespace MySite.Controllers
{
    public class WatchedController : Controller
    {
        private readonly WacthedService _watchedService;
        private readonly TmdbService _tmdbService;

        public WatchedController(ApplicationContext context, IOptions<Config> config)
        {
            _tmdbService = new TmdbService(config.Value);
            _watchedService = new WacthedService(context, _tmdbService);
        }

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
        public async Task<IActionResult> Create(CreateWatchedViewModel viewModel)
        {
            await _watchedService.Create(viewModel.Request);
            return Redirect("Index");
        }
    }
}
