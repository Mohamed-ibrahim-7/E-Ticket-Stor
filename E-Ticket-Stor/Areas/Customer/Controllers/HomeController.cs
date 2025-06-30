using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Ticket_Stor.Models;
using Microsoft.Extensions.Logging;
using E_Ticket_Stor.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket_Stor.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext _context = new ApplicationDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Cinema)
                
                .Include(m => m.Producer)
                .OrderByDescending(m => m.StartDate)
                .ToList();

            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Cinema)
               
                .Include(m => m.Producer)
                .Include(m => m.Actors_Movies)
                    .ThenInclude(am => am.Actor)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return View("NotFound");

            return View(movie);
        }
       
        public IActionResult BookTicket(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return View("NotFound");

            return View(movie);
        }

        
        [HttpPost]
   
        public IActionResult BookTicketConfirmed(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return View("NotFound");

            TempData["Success"] = $"✅ تم حجز تذكرة لفيلم {movie.Name} بنجاح!";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
