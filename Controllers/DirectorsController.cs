#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesWebApplication;
using ClosedXML.Excel;

namespace MoviesWebApplication.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly DBMoviesContext _context;

        public DirectorsController(DBMoviesContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Directors.ToListAsync());
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.DirectorId == id);
            if (director == null)
            {
                return NotFound();
            }

            //return View(director);
            return RedirectToAction("Index", "Movies", new { id = director.DirectorId, Name = director.Name });
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DirectorId, Name,Surname")] Director director)
        {
            IsExist(director);
            if (ModelState.IsValid)
            {
                if (_context.Directors.Count() == 0)//скинути ідент.
                {
                    _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Directors', RESEED, 0);");
                    _context.SaveChanges();
                }
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DirectorId,Name,Surname")] Director director)
        {
            if (id != director.DirectorId)
            {
                return NotFound();
            }
            IsExist(director);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.DirectorId))
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
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.DirectorId == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            var movieByDirector = await _context.Movies.Where(b => b.DirectorId == id).ToListAsync();//
            foreach(Movie movie in movieByDirector)//
            {//
                movie.Director = null;//
                movie.DirectorId = null;//
            }//


            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Director newDir;
                                string a = (worksheet.Name == "without director" ? " " : worksheet.Name);
                                var c =await _context.Directors.FirstOrDefaultAsync(g => ((g.Name.ToLower()+" "+g.Surname.ToLower()) == a.ToLower()));
                                    
                                if (c!=null)
                                {
                                    newDir = c;
                                }
                                else
                                {
                                    newDir = new Director();
                                    
                                    newDir.setNames(a);
                                   //newDir.Surname = " from EXCEL" ;

                                    //додати в контекст
                                    _context.Directors.Add(newDir);
                                }
                                //перегляд усіх рядків
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    

                                        var movie = await _context.Movies.FirstOrDefaultAsync(x => (
                                        x.Title == row.Cell(1).Value.ToString()&& x.LengthMinutes==Convert.ToInt32(row.Cell(3).Value.ToString())
                                        ));
                                        if (movie == null)
                                        {
                                            movie= new Movie();
                                            _context.Movies.Add(movie);
                                        }
                                        movie.Title = row.Cell(1).Value.ToString();
                                        movie.Description = row.Cell(2).Value.ToString();
                                        movie.LengthMinutes = Convert.ToInt32(row.Cell(3).Value.ToString());
                                        movie.Director = newDir;
                                        movie.DirectorId = newDir.DirectorId;
                                        
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        //for (int i = 2; i <= 5; i++)
                                        //{
                                        //    if (row.Cell(i).Value.ToString().Length > 0)
                                        //    {
                                        //        Author author;
                                        //        var a = (from aut in _context.Authors
                                        //                 where aut.Name.Contains(row.Cell(i).Value.ToString())
                                        //                 select aut).ToList();
                                        //        if (a.Count > 0)
                                        //        {
                                        //            author = a[0];
                                        //        }
                                        //        else
                                        //        {
                                        //            author = new Author();
                                        //            author.Name = row.Cell(i).Value.ToString();
                                        //            author.Info = " from EXCEL" ;
                                        //            //додати в контекст
                                        //            _context.Add(author);
                                        //        }
                                        //        AuthorBook ab = new AuthorBook();
                                        //        ab.Book = book;
                                        //        ab.Author = author;
                                        //        _context.AuthorBooks.Add(ab);
                                        //    }
                                        //}
                                    
                                    
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var directors = _context.Directors.Include("Movies").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проєктах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in directors)
                {
                    string t;
                    if (c.FullName == " ")
                    { t = "without director"; }
                    else
                    { t = c.FullName;  }
                    var worksheet = workbook.Worksheets.Add(t);
                    worksheet.Cell("A1").Value = "Title";
                    worksheet.Cell("B1").Value = "Description";     
                    worksheet.Cell("C1").Value = "LengthMinutes";
                    worksheet.Cell("D1").Value = "Director id";
                    worksheet.Cell("E1").Value = "Movie id";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var movies = c.Movies.ToList();
                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < movies.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = movies[i].Title;
                        worksheet.Cell(i + 2, 2).Value = movies[i].Description;
                        worksheet.Cell(i + 2, 3).Value = movies[i].LengthMinutes.ToString();
                        worksheet.Cell(i + 2, 4).Value = movies[i].DirectorId.ToString();
                        worksheet.Cell(i + 2, 5).Value = movies[i].MovieId.ToString();
                        //var ab = _context.AuthorBooks.Where(a => a.BookId == books[i].Id).Include(" Author ").ToList();
                        ////більше 4-ох нікуди писати
                        //int j = 0;
                        //foreach (var a in ab)
                        //{
                        //    if (j < 5)
                        //        {
                        //        worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                        //        j++;
                        //        }
                        //}
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                           {
                        //змініть назву файла відповідно до тематики Вашого проєкту
                        FileDownloadName = $"movies_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }




        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.DirectorId == id);
        }
        private void IsExist(Director director)
        {
            var a = _context.Directors.FirstOrDefault(g => (
            ((director.Name != null && g.Name.ToLower() == director.Name.ToLower())
            || (director.Name == null && g.Name == null))
            &&
            ((director.Surname != null && g.Surname.ToLower() == director.Surname.ToLower())
            ||( director.Surname == null&&g.Surname==null)))) ;

            if (a != null)
            {
                ModelState.AddModelError("Name", "Такий режисер вже існує");
                ModelState.AddModelError("Surname", "Такий режисер вже існує");
            }
        }
    }
}
