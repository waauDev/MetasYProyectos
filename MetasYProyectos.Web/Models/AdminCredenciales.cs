namespace MetasYProyectos.Web.Models
{
    public class AdminCredenciales
    {
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        public string RecoveryCodeHash { get; set; } = string.Empty;

        public string RecoveryCodeSalt { get; set; } = string.Empty;

        public bool Configurado { get; set; } = false;
    }
}
