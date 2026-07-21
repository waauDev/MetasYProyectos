using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Controllers
{
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    public class MetasProyectosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    
}
