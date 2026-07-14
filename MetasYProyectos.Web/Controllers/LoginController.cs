using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguracionService _configuracionService;

        public LoginController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CargarBasesDeDatos();
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel vm)
        {
            CargarBasesDeDatos();
            return View(vm);
        }

        private void CargarBasesDeDatos()
        {
            var configs = _configuracionService.ObtenerTodas();
            ViewBag.BasesDeDatos = configs.Select(c => c.Nombre).ToList();
        }
    }
}
