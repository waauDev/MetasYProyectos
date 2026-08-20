using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Domain.Entities;

namespace MetasYProyectos.Application.Mappings
{
    public static class PeriodoAdministrativoMapper
    {
        // ponytail: ID_COM_ENTIDAD fijo hasta que exista selección de entidad por sesión en el login.
        public const int IdComEntidadActual = 1;

        public static PeriodoAdministrativo ToEntity(PeriodoAdministrativoDto dto)
            => PeriodoAdministrativo.Crear(
                dto.IdPeriodo,
                IdComEntidadActual,
                dto.NitGobernante,
                dto.FechaInicio,
                dto.FechaFinal,
                dto.Cantidad,
                dto.NombreGobernante);

        public static PeriodoAdministrativoDto ToDto(PeriodoAdministrativo entidad)
            => new()
            {
                IdPeriodo = entidad.IdPeriodo,
                NitGobernante = entidad.NitGobernante,
                NombreGobernante = entidad.NombreGobernante,
                FechaInicio = entidad.FechaInicio,
                FechaFinal = entidad.FechaFinal,
                Cantidad = entidad.Cantidad
            };
    }
}
