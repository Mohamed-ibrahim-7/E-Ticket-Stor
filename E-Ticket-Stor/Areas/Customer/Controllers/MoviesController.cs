using E_Ticket_Stor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket_Stor.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MoviesController : Controller
    {
         ApplicationDbContext _context = new ApplicationDbContext();



        public IActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Cinema).ToList();
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
    }
}
