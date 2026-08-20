using MetasYProyectos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MetasYProyectos.Web.Autenticacion
{
    public sealed class RequierePermisoAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _codAccion;

        public RequierePermisoAttribute(string codAccion) => _codAccion = codAccion;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetRequiredService<IValidadorPermisoAccion>();

            if (!await validador.TienePermisoAsync(_codAccion, context.HttpContext.RequestAborted))
            {
                if (context.Controller is Controller controller)
                    controller.TempData["Error"] = "No tiene permisos para esta operación";

                context.Result = new RedirectToActionResult("Index", "DatosBasicos", new { area = "DatosBasicos" });
                return;
            }

            await next();
        }
    }
}
