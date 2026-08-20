using System.ComponentModel.DataAnnotations;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Web.ViewModels;

public sealed class PeriodoAdministrativoViewModel
{
    [Required(ErrorMessage = "El código del periodo es obligatorio.")]
    [Display(Name = "Código")]
    public int IdPeriodo { get; set; }

    [Required(ErrorMessage = "El NIT del gobernante es obligatorio.")]
    [Display(Name = "NIT gobernante")]
    public string NitGobernante { get; set; } = string.Empty;

    [Display(Name = "Nombre gobernante")]
    public string? NombreGobernante { get; set; }

    [Required(ErrorMessage = "La fecha inicial es obligatoria.")]
    [Display(Name = "Fecha inicial")]
    [DataType(DataType.Date)]
    public DateTime? FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha final es obligatoria.")]
    [Display(Name = "Fecha final")]
    [DataType(DataType.Date)]
    public DateTime? FechaFinal { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Display(Name = "Cantidad")]
    public int Cantidad { get; set; }

    // Distingue crear (true) de editar (false); el código no se puede cambiar en edición.
    public bool EsNuevo { get; set; } = true;

    public PeriodoAdministrativoDto ToDto() => new()
    {
        IdPeriodo = IdPeriodo,
        NitGobernante = NitGobernante.Trim(),
        FechaInicio = FechaInicio ?? default,
        FechaFinal = FechaFinal ?? default,
        Cantidad = Cantidad
    };

    public static PeriodoAdministrativoViewModel FromDto(PeriodoAdministrativoDto dto) => new()
    {
        IdPeriodo = dto.IdPeriodo,
        NitGobernante = dto.NitGobernante,
        NombreGobernante = dto.NombreGobernante,
        FechaInicio = dto.FechaInicio,
        FechaFinal = dto.FechaFinal,
        Cantidad = dto.Cantidad,
        EsNuevo = false
    };
}
