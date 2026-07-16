using System.ComponentModel.DataAnnotations;

namespace MetasYProyectos.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "La base de datos es obligatoria")]
        public string BaseDatos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo vigencia es obligatorio")]
        public string Vigencia { get; set; } = string.Empty;
    }
}
