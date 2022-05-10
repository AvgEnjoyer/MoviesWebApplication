using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBMoviesContext _context;
        public ChartController(DBMoviesContext context)
            {
            _context = context;
            }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var directors = _context.Directors.ToList();
            List<object> ofOne = new List<object>();
            ofOne.Add(new[] { "Режисер", "Кількість фільмів в базі" });
            int i = 0;
            foreach (var item in directors)
            {
                var a= _context.Movies.Where(x=>x.DirectorId == item.DirectorId).ToList();
                if (!(item.FullName == " "))
                    ofOne.Add(new object[] { item.FullName, a.Count() });
                else { i += a.Count(); }
            }
           
            ofOne.Add(new object[] { "Без Режисера", i });
            return new JsonResult(ofOne);
        }

        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var genres = _context.Genres.ToList();
            List<object> ofOne = new List<object>();
            ofOne.Add(new[] { "Жанр", "Кількість фільмів за жанром" });
            foreach (var item in genres)
            {
                var a = _context.GenresInMovies.Where(x => x.GenreId == item.GenreId).ToList();
                ofOne.Add(new object[] { item.Genre1, a.Count() });
            }
            return new JsonResult(ofOne);
        }


    }
}
