namespace MetasYProyectos.Domain.Filtros;

public sealed record PeriodoAdministrativoFiltro(
    int? IdPeriodoInicial,
    int? IdPeriodoFinal,
    string? NitGobernante);
