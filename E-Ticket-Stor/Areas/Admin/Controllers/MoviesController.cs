using E_Ticket_Stor.Data;
using E_Ticket_Stor.Models;
using E_Ticket_Stor.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace E_Ticket_Stor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index()
        {
            var allMovies = _context.Movies.Include(m => m.Cinema).ToList();
            return View(allMovies);
        }

        public IActionResult Filter(string searchString)
        {
            var allMovies = _context.Movies.Include(m => m.Cinema).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filtered = allMovies
                    .Where(m => m.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                                m.Description.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();

                return View("Index", filtered);
            }

            return View("Index", allMovies);
        }

        public IActionResult Details(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.Actors_Movies).ThenInclude(am => am.Actor)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return View("NotFound");

            return View(movie);
        }

        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewMovieVM movie, IFormFile ImageFile)
        {
            //if (!ModelState.IsValid)
            //{
                
            //    return View(movie);
            //}

            var newMovie = new Movie
            {
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                movieCategory = movie.MovieCategory,
                CinemaId = movie.CinemaId,
                ProducerId = movie.ProducerId
            };

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                newMovie.ImageURL = "/images/" + fileName;
            }

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            foreach (var actorId in movie.ActorIds)
            {
                _context.Actors_Movies.Add(new Actor_Movie
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Actors_Movies)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return View("NotFound");

            var response = new NewMovieVM
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                MovieCategory = movie.movieCategory,
                CinemaId = movie.CinemaId,
                ProducerId = movie.ProducerId,
                ActorIds = movie.Actors_Movies.Select(am => am.ActorId).ToList()
            };

            PopulateDropDownLists(response.ActorIds);
            return View(response);
        }

        [HttpPost]
        public IActionResult Edit(int id, NewMovieVM movie, IFormFile ImageFile)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                PopulateDropDownLists(movie.ActorIds);
                return View(movie);
            }

            var existingMovie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (existingMovie == null) return View("NotFound");

            existingMovie.Name = movie.Name;
            existingMovie.Description = movie.Description;
            existingMovie.Price = movie.Price;
            existingMovie.StartDate = movie.StartDate;
            existingMovie.EndDate = movie.EndDate;
            existingMovie.movieCategory = movie.MovieCategory;
            existingMovie.CinemaId = movie.CinemaId;
            existingMovie.ProducerId = movie.ProducerId;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                existingMovie.ImageURL = "/images/" + fileName;
            }

            _context.SaveChanges();

            var oldActors = _context.Actors_Movies.Where(am => am.MovieId == id).ToList();
            _context.Actors_Movies.RemoveRange(oldActors);
            _context.SaveChanges();

            foreach (var actorId in movie.ActorIds)
            {
                _context.Actors_Movies.Add(new Actor_Movie
                {
                    MovieId = id,
                    ActorId = actorId
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

       
        private void PopulateDropDownLists(List<int> selectedActorIds = null)
        {
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(_context.Producers, "Id", "FullName");
            ViewBag.Actors = new MultiSelectList(_context.Actors, "Id", "FullName", selectedActorIds);
        }
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return View("NotFound");

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return View("NotFound");

           
            var actorMovies = _context.Actors_Movies.Where(am => am.MovieId == id).ToList();
            _context.Actors_Movies.RemoveRange(actorMovies);

           
            if (!string.IsNullOrEmpty(movie.ImageURL))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", movie.ImageURL.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
