using MetasYProyectos.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MetasYProyectos.Web.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly AdminAuthService _authService;

        public AdminController(AdminAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (!_authService.EstaConfigurado())
                return RedirectToAction(nameof(Setup));
            return RedirectToAction(nameof(Login));
        }

        [HttpGet("setup")]
        public IActionResult Setup()
        {
            if (_authService.EstaConfigurado())
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost("setup")]
        public IActionResult Setup(string password, string confirmarPassword)
        {
            if (_authService.EstaConfigurado())
                return RedirectToAction(nameof(Login));

            if (string.IsNullOrWhiteSpace(password) || password != confirmarPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden");
                return View();

            }

            var recoveryCode = Guid.NewGuid().ToString("N")[..12];
            _authService.ConfigurarInicial(password, recoveryCode);

            return View("SetupCompletado", model: recoveryCode);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!_authService.EstaConfigurado())
                return RedirectToAction(nameof(Setup));
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string password)
        {
            if (!_authService.ValidarPassword(password))
            {
                ModelState.AddModelError("", "Contraseña incorrecta,");
                return View();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "admin") };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Configuracion");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));

        }

        [HttpGet("reset")]
        public IActionResult Reset() => View();

        [HttpPost("reset")]
        public IActionResult Reset(string recoveryCode, string nuevaPassword, string confirmarPassword)
        {
            if (nuevaPassword != confirmarPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden");
                return View();
            }

            try
            {
                var nuevoCodigo = _authService.RestablecerPassword(recoveryCode, nuevaPassword);
                return View("ResetCompletado", model: nuevoCodigo);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("", "Codigo de recuperacion invalido");
                return View();
            }
        }
    }

}
