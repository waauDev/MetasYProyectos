using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Areas.Reportes.Controllers
{
    [Area("Reportes")]
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    public class MetasYProyectosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
