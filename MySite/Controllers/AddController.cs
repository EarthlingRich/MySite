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
    public class AddController : Controller
    {
        private readonly MovieService _movieService;
        private readonly TmdbService _tmdbService;

        public AddController(ApplicationContext context, IOptions<Config> config)
        {
            _tmdbService = new TmdbService(config.Value);
            _movieService = new MovieService(context, _tmdbService);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> SearchMovies(string searchQuery)
        {
            var results = await _tmdbService.SearchAsync(searchQuery);

            var viewModel = new SelectMovieViewModel(results.ToList());
            return PartialView("SelectMovie", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectMovie(int tmdbId)
        {
            var tmdbMovie = await _tmdbService.GetMovieDetails(tmdbId);

            var viewModel = new AddMovieViewModel(tmdbMovie);
            return PartialView("AddMovie", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel viewModel)
        {
            await _movieService.AddMovie(viewModel.Request);
            return Redirect("Index");
        }
    }
}
