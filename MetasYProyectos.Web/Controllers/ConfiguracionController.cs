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
            => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _mediator.Send(new ObtenerConfiguracionQuery());
            var vms = lista.Select(ConfiguracionViewModel.FromDto).ToList();
            return View(vms);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Editar", new ConfiguracionViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string nombre)
        {
            var dto = await _mediator.Send(new ObtenerConfiguracionPorNombreQuery(nombre));
            if (dto is null)
                return RedirectToAction(nameof(Index));

            return View(ConfiguracionViewModel.FromDto(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(ConfiguracionViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Editar", vm);

            var resultado = await _mediator.Send(new GuardarConfiguracionCommand(vm.ToDto()));
            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string nombre)
        {
            var resultado = await _mediator.Send(new EliminarConfiguracionCommand(nombre));
            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProbarConexion(ConfiguracionViewModel vm)
        {
            var resultado = await _mediator.Send(new ProbarConexionQuery(vm.ToDto()));
            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;
            return View("Editar", vm);
        }
    }
}
