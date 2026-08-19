using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Areas.DatosBasicos.Controllers
{
    [Area("DatosBasicos")]
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    public class PeriodosAdministrativosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
