using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Exceptions;
using Xunit;

namespace MetasYProyectos.Test;

public class PeriodoAdministrativoTests
{
    [Fact]
    public void Crear_con_datos_validos_asigna_propiedades()
    {
        var periodo = PeriodoAdministrativo.Crear(
            2026, 1, "900123456", new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), 4, "Alcaldía de prueba");

        Assert.Equal(2026, periodo.IdPeriodo);
        Assert.Equal("900123456", periodo.NitGobernante);
        Assert.Equal(4, periodo.Cantidad);
        Assert.Equal("Alcaldía de prueba", periodo.NombreGobernante);
    }

    [Fact]
    public void Crear_con_fecha_final_anterior_a_inicial_lanza_excepcion()
    {
        Assert.Throws<PeriodoAdministrativoInvalidoException>(() =>
            PeriodoAdministrativo.Crear(
                2026, 1, "900123456", new DateTime(2026, 12, 31), new DateTime(2026, 1, 1), 4));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Crear_con_id_periodo_invalido_lanza_excepcion(int idPeriodo)
    {
        Assert.Throws<PeriodoAdministrativoInvalidoException>(() =>
            PeriodoAdministrativo.Crear(
                idPeriodo, 1, "900123456", new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), 4));
    }

    [Fact]
    public void Crear_con_nit_vacio_lanza_excepcion()
    {
        Assert.Throws<PeriodoAdministrativoInvalidoException>(() =>
            PeriodoAdministrativo.Crear(
                2026, 1, "  ", new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), 4));
    }

    [Fact]
    public void Crear_con_cantidad_cero_lanza_excepcion()
    {
        Assert.Throws<PeriodoAdministrativoInvalidoException>(() =>
            PeriodoAdministrativo.Crear(
                2026, 1, "900123456", new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), 0));
    }
}
