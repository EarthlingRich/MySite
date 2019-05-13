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
    public class ReadController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly GoodreadsService _goodreadsService;
        private readonly ReadService _readService;

        public ReadController(ApplicationContext context, IOptions<Config> config)
        {
            _context = context;
            _goodreadsService = new GoodreadsService(config.Value);
            _readService = new ReadService(_context, _goodreadsService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string searchQuery)
        {
            var results = await _goodreadsService.SearchAsync(searchQuery);

            var viewModel = new SelectReadViewModel(results.ToList());
            return PartialView("Select", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Select(int goodreadsId)
        {
            var goodreadsBook = await _goodreadsService.GetBookDetails(goodreadsId);

            var viewModel = new CreateReadViewModel(goodreadsBook);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> SelectForUpdate(int readId)
        {
            var read = await _context.Read.FindAsync(readId);
            var viewModel = new CreateReadViewModel(read);
            return PartialView("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(CreateReadViewModel viewModel)
        {
            await _readService.CreateOrUpdate(viewModel.Request);
            return Redirect("Index");
        }
    }

    public class ReadListViewComponent : ViewComponent
    {
        const int ListPageSize = 20;
        private readonly ApplicationContext _context;

        public ReadListViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page)
        {
            var readItems = await _context.Read.OrderByDescending(_ => _.Id).Skip(ListPageSize * (page - 1)).Take(ListPageSize).ToListAsync();
            var viewModel = new List<ListReadViewModel>();
            foreach (var read in readItems)
            {
                viewModel.Add(new ListReadViewModel(read));
            }

            return View("~/Views/Read/List.cshtml", viewModel);
        }
    }
}
