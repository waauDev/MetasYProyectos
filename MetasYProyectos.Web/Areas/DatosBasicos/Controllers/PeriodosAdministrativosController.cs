using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands;
using MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries;
using MetasYProyectos.Web.Autenticacion;
using MetasYProyectos.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MetasYProyectos.Web.Areas.DatosBasicos.Controllers
{
    [Area("DatosBasicos")]
    [Authorize(AuthenticationSchemes = EsquemasAutenticacion.UsuarioOracle)]
    [RequierePermiso("981001")]
    public class PeriodosAdministrativosController : Controller
    {
        private readonly IMediator _mediator;

        public PeriodosAdministrativosController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Index(
            PeriodoAdministrativoFiltroViewModel filtro, int pagina = 1, int? id = null, CancellationToken ct = default)
        {
            PeriodoAdministrativoViewModel form;
            if (id.HasValue)
            {
                var dto = await _mediator.Send(new ObtenerPeriodoQuery(id.Value), ct);
                if (dto is null)
                {
                    TempData["Error"] = "El periodo no existe";
                    form = new PeriodoAdministrativoViewModel { EsNuevo = true };
                }
                else
                {
                    form = PeriodoAdministrativoViewModel.FromDto(dto);
                }
            }
            else
            {
                form = new PeriodoAdministrativoViewModel { EsNuevo = true };
            }

            return View(await ConstruirListaAsync(filtro, pagina, form, ct));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(PeriodoAdministrativoViewModel form, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var resultado = form.EsNuevo
                    ? await _mediator.Send(new CrearPeriodoCommand(form.ToDto()), ct)
                    : await _mediator.Send(new ActualizarPeriodoCommand(form.ToDto()), ct);

                if (resultado.Exitoso)
                {
                    TempData["Ok"] = resultado.Mensaje;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, resultado.Mensaje);
            }

            var modelo = await ConstruirListaAsync(new PeriodoAdministrativoFiltroViewModel(), 1, form, ct);
            return View(nameof(Index), modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            var resultado = await _mediator.Send(new EliminarPeriodoCommand(id), ct);
            TempData[resultado.Exitoso ? "Ok" : "Error"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarNit(string texto, CancellationToken ct)
        {
            var resultado = await _mediator.Send(new BuscarNitQuery(texto), ct);
            return Json(resultado);
        }

        private async Task<PeriodoAdministrativoListaViewModel> ConstruirListaAsync(
            PeriodoAdministrativoFiltroViewModel filtro, int pagina, PeriodoAdministrativoViewModel form, CancellationToken ct)
        {
            var resultado = await _mediator.Send(new BuscarPeriodosQuery(
                filtro.IdPeriodoInicial, filtro.IdPeriodoFinal, filtro.NitGobernante, pagina), ct);

            return new PeriodoAdministrativoListaViewModel
            {
                Items = resultado.Items.Select(PeriodoAdministrativoViewModel.FromDto).ToList(),
                Filtro = filtro,
                Pagina = resultado.Pagina,
                TamanoPagina = resultado.TamanoPagina,
                Total = resultado.Total,
                Form = form
            };
        }
    }
}
