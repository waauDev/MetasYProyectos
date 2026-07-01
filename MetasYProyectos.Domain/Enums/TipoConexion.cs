namespace MetasYProyectos.Domain.Enums;

/// <summary>
/// Tipo de conexión a Oracle.
/// ServiceName: recomendado para Oracle 19c y 21c.
/// SID: usado en versiones anteriores (9, 10, 11).
/// TNS: usa el archivo tnsnames.ora del cliente Oracle.
/// </summary>
public enum TipoConexion
{
    ServiceName = 0,
    SID = 1,
    TNS = 2
}