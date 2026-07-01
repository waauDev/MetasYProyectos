using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
