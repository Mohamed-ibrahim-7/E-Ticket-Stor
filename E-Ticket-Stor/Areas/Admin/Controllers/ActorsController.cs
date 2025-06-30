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
    public class ActorsController : Controller
    {
        ApplicationDbContext _context = new();

      
        public IActionResult Index()
        {
            var actors = _context.Actors;
            return View(actors.ToList());
        }

        // GET: /Admin/Actor/Details/5
        public IActionResult Details(int id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);
            if (actor == null) return View("NotFound");

            return View(actor);
        }

        // GET: /Admin/Actor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Actor/Create
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile ImageFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(actor);
            //}

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                actor.ProfilePictureURL = fileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please upload a profile picture.");
                return View(actor);
            }

            _context.Actors.Add(actor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Actor/Edit/5
        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);
            if (actor == null) return View("NotFound");

            return View(actor);
        }

        // POST: /Admin/Actor/Edit/5
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile ImageFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(actor);
            //}

            var existingActor = _context.Actors.FirstOrDefault(a => a.Id == actor.Id);
            if (existingActor == null) return View("NotFound");

            existingActor.FullName = actor.FullName;
            existingActor.Bio = actor.Bio;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                existingActor.ProfilePictureURL = fileName;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Actor/Delete/5
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);
            if (actor == null) return View("NotFound");

            return View(actor);
        }

        // POST: /Admin/Actor/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);
            if (actor == null) return View("NotFound");

            _context.Actors.Remove(actor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
