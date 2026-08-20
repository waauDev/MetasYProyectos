using System.ComponentModel.DataAnnotations;

namespace MetasYProyectos.Web.ViewModels;

public sealed class PeriodoAdministrativoFiltroViewModel
{
    [Display(Name = "Código inicial")]
    public int? IdPeriodoInicial { get; set; }

    [Display(Name = "Código final")]
    public int? IdPeriodoFinal { get; set; }

    [Display(Name = "NIT gobernante")]
    public string? NitGobernante { get; set; }
}
