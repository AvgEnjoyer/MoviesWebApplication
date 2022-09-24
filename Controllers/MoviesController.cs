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
    public class MoviesController : Controller
    {
        private readonly DBMoviesContext _context;

        public MoviesController(DBMoviesContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int? id,string? name)
        {
            //if(id==null)return RedirectToAction("Directors","Index");
            if (id == null)
            {
                var dBMoviesContext = _context.Movies.Include(m => m.Director);
                return View(await dBMoviesContext.ToListAsync());
            }

            ViewBag.Id = id;
            ViewBag.Name = name;

            var moviesByDirector =_context.Movies.Where(b=>b.DirectorId==id).Include(b=>b.Director);
            
            return View(await moviesByDirector.ToListAsync());
         // return View(await dBMoviesContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }
            
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["DirectorId"] = new SelectList(_context.Directors.ToList().OrderBy(x=>x.FullName), "DirectorId", "FullName");
            return View();
        }
        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Description,LengthMinutes,DirectorId")] Movie movie)
        {

            _updateItem(movie);
            IsExist(movie);
            if (ModelState.IsValid)
            {
                if (_context.Movies.Count() == 0)//скинути ідент.
                { 
                    _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Movies', RESEED, 0);");
                    _context.SaveChanges();
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors.ToList().OrderBy(x => x.FullName), "DirectorId", "FullName", movie.DirectorId);

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors.ToList().OrderBy(x => x.FullName), "DirectorId", "FullName", movie.DirectorId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Description,LengthMinutes,DirectorId")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }
            _updateItem(movie);
            IsExist(movie);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            ViewData["DirectorId"] = new SelectList(_context.Directors.ToList().OrderBy(x => x.FullName), "DirectorId", "FullName", movie.DirectorId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _delete(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }

        public void _delete(int id)
        {
            var movie = _context.Movies.Find(id);
            

            var actorsINmovies_records = _context.ActorsInMovies.Where(x => x.MovieId == id).ToList();
            var genresINmovies_records = _context.GenresInMovies.Where(x => x.MovieId == id).ToList();
            foreach (var item in actorsINmovies_records)
            {
                new ActorsInMoviesController(_context)._delete(item.Id);
            }
            foreach(var item in genresINmovies_records)
            {
                new GenresInMoviesController(_context)._delete(item.Id);
            }
            _context.Movies.Remove(movie);
            
        }
        public void _updateItem(Movie movie)
        {
            
            var director = _context.Directors.FirstOrDefault(m => m.DirectorId == movie.DirectorId);
            movie.Director = director;
        }
        private void IsExist(Movie movie)
        {
            var a = _context.Movies.FirstOrDefault(g => (g.Title.ToLower() == movie.Title.ToLower() && g.DirectorId!=null&&g.DirectorId==movie.DirectorId));
            

            if (a != null)
            { 
                if (a.MovieId != movie.MovieId)
                {
                    ModelState.AddModelError("Title", "Такий фільм з таким режисером вже існує");
                    ModelState.AddModelError("DirectorId", "Такий фільм з таким режисером вже існує");
                }
            _context.Entry(a).State = EntityState.Detached;
            }
        }
    }
}
