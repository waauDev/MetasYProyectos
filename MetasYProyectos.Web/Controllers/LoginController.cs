using MetasYProyectos.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.BasesDeDatos = new List<string> { "MetasYProyectos_Prod", "MetasYProyectos_Test" };
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel vm)
        {
            ViewBag.BasesDeDatos = new List<string> { "MetasYProyectos_Prod", "MetasYProyectos_Test" };
            return View(vm);
        }
       
    }
}
