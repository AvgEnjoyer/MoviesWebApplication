using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesWebApplication.Controllers
{
    public class GameProject : Controller
    {
        // GET: GameProject
        public IActionResult Index()
        {
            return View();
        }
    }
}
