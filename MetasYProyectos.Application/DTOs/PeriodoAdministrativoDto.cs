namespace MetasYProyectos.Application.DTOs
{
    public sealed record PeriodoAdministrativoDto
    {
        public int IdPeriodo { get; set; }
        public string NitGobernante { get; set; } = string.Empty;
        public string? NombreGobernante { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int Cantidad { get; set; }
    }
}
