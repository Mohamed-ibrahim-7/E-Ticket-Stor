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
    public class ProducersController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var Producers = _context.Producers;
            return View(Producers.ToList());
        }
        public IActionResult Details(int id)
        {
            var producer = _context.Producers.FirstOrDefault(p => p.Id == id);
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }




        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Producer producer, IFormFile ImageFile)
        {    
            //if(!ModelState.IsValid)
            //{
            //    return View(producer);
            //}
            if (ImageFile is not null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                // Save img in wwwroot
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }

                // Save img in DB
                producer.ProfilePictureURL = fileName;

                // Save product in DB
                _context.Add(producer);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
          return BadRequest();

        }
        public IActionResult Edit(int id)
        {
            var producer = _context.Producers.FirstOrDefault(p => p.Id == id);
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }
        [HttpPost]
        public IActionResult Edit (Producer producer, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(producer);
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                producer.ProfilePictureURL = fileName;
            }
            _context.Producers.Update(producer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // GET: Producers/Delete/5
        public IActionResult Delete(int id)
        {
            var producer = _context.Producers.FirstOrDefault(p => p.Id == id);
            if (producer == null) return View("NotFound");
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var producer = _context.Producers.FirstOrDefault(p => p.Id == id);
            if (producer == null) return View("NotFound");

            _context.Producers.Remove(producer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}
