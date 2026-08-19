using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Areas.Consultas.Controllers
{
    [Area("Consultas")]
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    public class ProcesosContractualesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
