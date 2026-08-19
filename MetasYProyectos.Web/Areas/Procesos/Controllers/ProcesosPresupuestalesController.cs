using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Areas.Procesos.Controllers
{
    [Area("Procesos")]
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    public class ProcesosPresupuestalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
