using E_Ticket_Stor.Data;
using E_Ticket_Stor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace E_Ticket_Stor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        // GET: /Admin/Cinema
        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.ToList();
            return View(cinemas);
        }

        // GET: /Admin/Cinema/Details/5
        public IActionResult Details(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return View("NotFound");

            return View(cinema);
        }

        // GET: /Admin/Cinema/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Cinema/Create
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile ImageFile)
        {
            //if (!ModelState.IsValid)
            //    return View(cinema);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                cinema.Logo = "/images/" + fileName;
            }
          

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Cinema/Edit/5
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return View("NotFound");

            return View(cinema);
        }

        // POST: /Admin/Cinema/Edit/5
        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile ImageFile)
        {
            //if (!ModelState.IsValid)
            //    return View(cinema);

            var existingCinema = _context.Cinemas.FirstOrDefault(c => c.Id == cinema.Id);
            if (existingCinema == null) return View("NotFound");

            existingCinema.Name = cinema.Name;
            existingCinema.Description = cinema.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                existingCinema.Logo = "/images/" + fileName;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Cinema/Delete/5
        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return View("NotFound");

            return View(cinema);
        }

        // POST: /Admin/Cinema/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return View("NotFound");

            // حذف الصورة من wwwroot
            if (!string.IsNullOrEmpty(cinema.Logo))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cinema.Logo.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
