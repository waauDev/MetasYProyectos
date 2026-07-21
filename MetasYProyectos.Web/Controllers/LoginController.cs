using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Domain.Autenticacion;
using MetasYProyectos.Web.Autenticacion;
using MetasYProyectos.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MetasYProyectos.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IConfiguracionService _configuracionService;
        private readonly IMediator _mediator;

        public LoginController(IConfiguracionService configuracionService, IMediator mediator)
        {
            _configuracionService = configuracionService;
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            CargarBasesDeDatos();
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(Index));
            }

            var credenciales = new CredencialesLogin(vm.Usuario.Trim(), vm.Contrasena, vm.Vigencia.Trim(), vm.BaseDatos);
            var resultado = await _mediator.Send(new AutenticarUsuarioCommand(credenciales), ct);

            if (!resultado.Existoso)
            {
                TempData["Error"] = resultado.MensajeError;
                return RedirectToAction(nameof(Index));
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, credenciales.Usuario),
                new("BaseDatos", credenciales.BaseDatos),
                new("Vigencia", credenciales.vigencia)
            };
            var identidad = new ClaimsIdentity(claims, EsquemasAutenticacion.UsuarioOracle);
            await HttpContext.SignInAsync(EsquemasAutenticacion.UsuarioOracle, new ClaimsPrincipal(identidad));

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(EsquemasAutenticacion.UsuarioOracle);
            return RedirectToAction(nameof(Index));
        }

        private void CargarBasesDeDatos()
        {
            ViewBag.BasesDeDatos = _configuracionService.ObtenerTodas().Select(c => c.Nombre).ToList();
        }
    }
}
