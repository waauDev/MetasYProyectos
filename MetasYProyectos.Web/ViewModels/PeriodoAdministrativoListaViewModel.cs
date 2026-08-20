namespace MetasYProyectos.Web.ViewModels;

public sealed class PeriodoAdministrativoListaViewModel
{
    public List<PeriodoAdministrativoViewModel> Items { get; set; } = new();
    public PeriodoAdministrativoFiltroViewModel Filtro { get; set; } = new();
    public PeriodoAdministrativoViewModel Form { get; set; } = new() { EsNuevo = true };
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 15;
    public int Total { get; set; }
    public int TotalPaginas => Total == 0 ? 1 : (int)Math.Ceiling(Total / (double)TamanoPagina);
}
