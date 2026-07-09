
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.UseCases.Configuracion.Commands;
using MetasYProyectos.Application.UseCases.Configuracion.Queries;
using MetasYProyectos.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Controllers
{
    [Authorize]
    public class ConfiguracionController : Controller
    {
        private readonly IMediator _mediator;

        public ConfiguracionController(IMediator mediator) 
            => _mediator=mediator;       

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dto = await _mediator.Send(new ObtenerConfiguracionQuery());
            var vm = dto is null
                ? new ConfiguracionViewModel()
                : ConfiguracionViewModel.FromDto(dto);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(ConfiguracionViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Index", vm);

            var resultado = await _mediator.Send(new GuardarConfiguracionCommand(vm.ToDto()));

            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProbarConexion(ConfiguracionViewModel vm)
        {
            var resultado = await _mediator.Send(new ProbarConexionQuery(vm.ToDto()));

            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;
            vm.ConfiguracionGuardada = true;
            return View("Index", vm);

        }
    }

}
