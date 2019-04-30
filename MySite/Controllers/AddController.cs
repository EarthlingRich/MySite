using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySite.Models;
using MySite.Services;

namespace MySite.Controllers
{
    public class AddController : Controller
    {
        readonly TmdbService _tmdbService;

        public AddController(IOptions<Config> config)
        {
            _tmdbService = new TmdbService(config.Value);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectMovie(string searchQuery)
        {
            var results = await _tmdbService.SearchAsync(searchQuery);

            var viewModel = new AddMovieViewModel(results.ToList());
            return PartialView(viewModel);
        }
    }
}
