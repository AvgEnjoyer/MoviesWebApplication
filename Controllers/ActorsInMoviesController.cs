#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesWebApplication;

namespace MoviesWebApplication.Controllers
{
    public class ActorsInMoviesController : Controller
    {
        private readonly DBMoviesContext _context;

        public ActorsInMoviesController(DBMoviesContext context)
        {
            _context = context;
        }

        // GET: ActorsInMovies
        public async Task<IActionResult> Index()
        {
            var dBMoviesContext = _context.ActorsInMovies.Include(a => a.Actor).Include(a => a.Movie);
            return View(await dBMoviesContext.ToListAsync());
        }

        // GET: ActorsInMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsInMovie = await _context.ActorsInMovies
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorsInMovie == null)
            {
                return NotFound();
            }

            return View(actorsInMovie);
        }

        // GET: ActorsInMovies/Create
        public IActionResult Create()
        {
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "FullName");
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title");
            return View();
        }

        // POST: ActorsInMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActorId,MovieId")] ActorsInMovie actorsInMovie)
        {
            IsExist(actorsInMovie);
            _updateItem(actorsInMovie);
            if (ModelState.IsValid)
            {
                _context.Add(actorsInMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "FullName", actorsInMovie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", actorsInMovie.MovieId);
            return View(actorsInMovie);
        }

        // GET: ActorsInMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsInMovie = await _context.ActorsInMovies.FindAsync(id);
            if (actorsInMovie == null)
            {
                return NotFound();
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "FullName", actorsInMovie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", actorsInMovie.MovieId);
            return View(actorsInMovie);
        }

        // POST: ActorsInMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActorId,MovieId")] ActorsInMovie actorsInMovie)
        {
            if (id != actorsInMovie.Id)
            {
                return NotFound();
            }
            IsExist(actorsInMovie);
            _updateItem(actorsInMovie);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actorsInMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsInMovieExists(actorsInMovie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "ActorId", actorsInMovie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", actorsInMovie.MovieId);
            return View(actorsInMovie);
        }

        // GET: ActorsInMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsInMovie = await _context.ActorsInMovies
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorsInMovie == null)
            {
                return NotFound();
            }

            return View(actorsInMovie);
        }

        // POST: ActorsInMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _delete(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorsInMovieExists(int id)
        {
            return _context.ActorsInMovies.Any(e => e.Id == id);
        }
        public void _delete(int id)
        {
            var actorsInMovie = _context.ActorsInMovies.Find(id);
            _context.ActorsInMovies.Remove(actorsInMovie);

        }
        public void _updateItem(ActorsInMovie actorsInMovie)
        {
            var actor = _context.Actors.FirstOrDefault(m => m.ActorId == actorsInMovie.ActorId);
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == actorsInMovie.MovieId);
            //actor.ActorsInMovies.Add(actorsInMovie);
            //movie.ActorsInMovies.Add(actorsInMovie);
            actorsInMovie.Movie = movie;
            actorsInMovie.Actor = actor;
            //_context.Update(actor);
            //_context.Update(movie);
            //_context.SaveChanges();
        }
        private void IsExist(ActorsInMovie actorsInMovie) {
            var a = _context.ActorsInMovies.FirstOrDefault(g => (g.ActorId == actorsInMovie.ActorId && g.MovieId == actorsInMovie.MovieId));
            if (a != null)
            {
                ModelState.AddModelError("ActorId", "Такий запис вже існує");
                ModelState.AddModelError("MovieId", "Такий запис вже існує");
            }
        }

}
}
