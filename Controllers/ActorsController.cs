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
    public class ActorsController : Controller
    {
        private readonly DBMoviesContext _context;

        public ActorsController(DBMoviesContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,Name,Surname,BirthDate,HasOscar")] Actor actor)
        {
            if(DateTime.Now.Year.CompareTo(actor.BirthDate.Year)<=0)
            { ModelState.AddModelError("BirthDate", "Час задано некоректно"); }
            IsExist(actor);
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,Name,Surname,BirthDate,HasOscar")] Actor actor)
        {

            if (id != actor.ActorId)
            {
                return NotFound();
            }
            if (DateTime.Now.Year.CompareTo(actor.BirthDate.Year) <= 0)
            { ModelState.AddModelError("BirthDate", "Час задано некоректно"); }
            IsExist(actor);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             _delete(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
        public void _delete(int id)
        {
            var actor = _context.Actors.Find(id);

            var actorsINmovies_records = _context.ActorsInMovies.Where(x => x.ActorId == id).ToList();
            foreach (var item in actorsINmovies_records)
            {
                new ActorsInMoviesController(_context)._delete(item.Id);
            }
            _context.Actors.Remove(actor);

        }
        private void IsExist(Actor actor)
        {

            var a = _context.Actors.FirstOrDefault(g => (
            g.Name.ToLower() == actor.Name.ToLower()&&
            g.Surname.ToLower()==actor.Surname.ToLower() && 
            g.BirthDate.Year.Equals(actor.BirthDate.Year)
            )
            );
            if (a != null)
            {
                ModelState.AddModelError("BirthDate", "Такий актор вже існує");
                ModelState.AddModelError("Name", "Такий актор вже існує");
                ModelState.AddModelError("Surname", "Такий актор вже існує");
            }
        }
    }
}
