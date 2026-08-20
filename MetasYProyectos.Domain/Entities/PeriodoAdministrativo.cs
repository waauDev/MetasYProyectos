using MetasYProyectos.Domain.Exceptions;

namespace MetasYProyectos.Domain.Entities;

public class PeriodoAdministrativo
{
    public int IdPeriodo { get; private set; }
    public int IdComEntidad { get; private set; }
    public string NitGobernante { get; private set; } = string.Empty;
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFinal { get; private set; }
    public int Cantidad { get; private set; }
    public string? NombreGobernante { get; private set; }

    private PeriodoAdministrativo() { }

    public static PeriodoAdministrativo Crear(
        int idPeriodo,
        int idComEntidad,
        string nitGobernante,
        DateTime fechaInicio,
        DateTime fechaFinal,
        int cantidad,
        string? nombreGobernante = null)
    {
        if (idPeriodo <= 0)
            throw new PeriodoAdministrativoInvalidoException(nameof(IdPeriodo), "El código del periodo es obligatorio");

        if (string.IsNullOrWhiteSpace(nitGobernante))
            throw new PeriodoAdministrativoInvalidoException(nameof(NitGobernante), "El NIT del gobernante es obligatorio");

        if (fechaInicio == default)
            throw new PeriodoAdministrativoInvalidoException(nameof(FechaInicio), "La fecha inicial es obligatoria");

        if (fechaFinal == default)
            throw new PeriodoAdministrativoInvalidoException(nameof(FechaFinal), "La fecha final es obligatoria");

        if (fechaFinal < fechaInicio)
            throw new PeriodoAdministrativoInvalidoException(nameof(FechaFinal), "La fecha final no puede ser anterior a la fecha inicial");

        if (cantidad <= 0)
            throw new PeriodoAdministrativoInvalidoException(nameof(Cantidad), "La cantidad debe ser mayor a cero");

        return new PeriodoAdministrativo
        {
            IdPeriodo = idPeriodo,
            IdComEntidad = idComEntidad,
            NitGobernante = nitGobernante.Trim(),
            FechaInicio = fechaInicio.Date,
            FechaFinal = fechaFinal.Date,
            Cantidad = cantidad,
            NombreGobernante = nombreGobernante
        };
    }
}
