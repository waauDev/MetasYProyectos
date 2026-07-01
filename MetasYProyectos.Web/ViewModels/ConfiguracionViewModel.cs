using System.ComponentModel.DataAnnotations;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Domain.Enums;

namespace MetasYProyectos.Web.ViewModels;

/// <summary>
/// ViewModel exclusivo para la vista de configuración.
/// Contiene atributos de validación de UI (DataAnnotations)
/// que no pertenecen al dominio ni a Application.
/// </summary>
public sealed class ConfiguracionViewModel
{
    [Required(ErrorMessage = "El servidor es obligatorio.")]
    [Display(Name = "Servidor / Host")]
    public string Servidor { get; set; } = string.Empty;

    [Required(ErrorMessage = "El puerto es obligatorio.")]
    [Display(Name = "Puerto")]
    public string Puerto { get; set; } = "1521";

    [Required(ErrorMessage = "El Service Name / SID es obligatorio.")]
    [Display(Name = "Service Name / SID / TNS")]
    public string Servicio { get; set; } = string.Empty;

    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [Display(Name = "Usuario")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Versión de Oracle")]
    public VersionOracle Version { get; set; } = VersionOracle.Oracle19;

    [Display(Name = "Tipo de conexión")]
    public TipoConexion TipoConexion { get; set; } = TipoConexion.ServiceName;

    // Estado, no editable por el usuario
    public bool ConfiguracionGuardada { get; set; }

    // ── Mapeos ────────────────────────────────────────────────

    public ConfiguracionBDDto ToDto() => new()
    {
        Servidor = Servidor,
        Puerto = Puerto,
        Servicio = Servicio,
        Usuario = Usuario,
        Password = Password,
        Version = Version,
        TipoConexion = TipoConexion
    };

    public static ConfiguracionViewModel FromDto(ConfiguracionBDDto dto) => new()
    {
        Servidor = dto.Servidor,
        Puerto = dto.Puerto,
        Servicio = dto.Servicio,
        Usuario = dto.Usuario,
        Password = dto.Password, // ya viene enmascarado desde Application
        Version = dto.Version,
        TipoConexion = dto.TipoConexion,
        ConfiguracionGuardada = true
    };
}