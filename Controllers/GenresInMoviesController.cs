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
    public class GenresInMoviesController : Controller
    {
        private readonly DBMoviesContext _context;

        public GenresInMoviesController(DBMoviesContext context)
        {
            _context = context;
        }

        // GET: GenresInMovies
        public async Task<IActionResult> Index()
        {
            var dBMoviesContext = _context.GenresInMovies.Include(g => g.Genre).Include(g => g.Movie);
            return View(await dBMoviesContext.ToListAsync());
        }

        // GET: GenresInMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresInMovie = await _context.GenresInMovies
                .Include(g => g.Genre)
                .Include(g => g.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genresInMovie == null)
            {
                return NotFound();
            }

            return View(genresInMovie);
        }

        // GET: GenresInMovies/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId");
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId");
            return View();
        }

        // POST: GenresInMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenreId,MovieId")] GenresInMovie genresInMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genresInMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", genresInMovie.GenreId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", genresInMovie.MovieId);
            return View(genresInMovie);
        }

        // GET: GenresInMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresInMovie = await _context.GenresInMovies.FindAsync(id);
            if (genresInMovie == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", genresInMovie.GenreId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", genresInMovie.MovieId);
            return View(genresInMovie);
        }

        // POST: GenresInMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreId,MovieId")] GenresInMovie genresInMovie)
        {
            if (id != genresInMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genresInMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenresInMovieExists(genresInMovie.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", genresInMovie.GenreId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", genresInMovie.MovieId);
            return View(genresInMovie);
        }

        // GET: GenresInMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresInMovie = await _context.GenresInMovies
                .Include(g => g.Genre)
                .Include(g => g.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genresInMovie == null)
            {
                return NotFound();
            }

            return View(genresInMovie);
        }

        // POST: GenresInMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genresInMovie = await _context.GenresInMovies.FindAsync(id);
            _context.GenresInMovies.Remove(genresInMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenresInMovieExists(int id)
        {
            return _context.GenresInMovies.Any(e => e.Id == id);
        }
    }
}
