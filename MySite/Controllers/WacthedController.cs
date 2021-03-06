﻿using System.Collections.Generic;
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
        public async Task<PartialViewResult> Search(string searchQuery, WatchedType watchedType)
        {
            SelectWatchedViewModel viewModel;
            if(watchedType == WatchedType.Movie)
            {
                var results = await _tmdbService.SearchMovie(searchQuery);
                viewModel = new SelectWatchedViewModel(results.ToList());
            }
            else
            {
                var results = await _tmdbService.SearchSerie(searchQuery);
                viewModel = new SelectWatchedViewModel(results.ToList());
            }

            return PartialView("Select", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Select(int tmdbId, WatchedType watchedType)
        {
            var tmdbWatchedResponse = await _tmdbService.GetDetails(tmdbId, null, watchedType);
            var viewModel = new CreateWatchedViewModel(tmdbWatchedResponse, watchedType);

            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectForUpdate(int watchedId)
        {
            var watched = await _context.Watched.FindAsync(watchedId);
            var tmdbWatchedResponse = await _tmdbService.GetDetails(watched.TmdbId, watched.SeasonNumber, watched.WatchedType);

            var viewModel = new CreateWatchedViewModel(tmdbWatchedResponse, watched);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(CreateWatchedViewModel viewModel)
        {
            await _watchedService.CreateOrUpdate(viewModel.Request);
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int watchedId)
        {
            await _watchedService.Delete(watchedId);
            return Ok();
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
