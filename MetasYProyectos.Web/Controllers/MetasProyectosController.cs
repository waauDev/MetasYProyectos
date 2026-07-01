using MetasYProyectos.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MetasYProyectos.Web.Controllers
{
    
    public class MetasProyectosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    
}
