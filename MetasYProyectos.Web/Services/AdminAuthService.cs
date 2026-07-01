using MetasYProyectos.Web.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace MetasYProyectos.Web.Services
{
    public class AdminAuthService
    {
        private readonly string _filePath;

        public AdminAuthService(IWebHostEnvironment env)
        {
            var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "admin.json");

        }

        public AdminCredenciales Cargar()
        {
            if (!File.Exists(_filePath))
                return new AdminCredenciales();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AdminCredenciales>(json) ?? new AdminCredenciales();
        }

        private void Guardar(AdminCredenciales credenciales)
        {
            var json = JsonSerializer.Serialize(credenciales, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }

        public bool EstaConfigurado() => Cargar().Configurado;    

        public void ConfigurarInicial(string password, string recoveryCode)
        {
            var (hash, salt) = HashearValor(password);
            var (recHash, recSalt) = HashearValor(recoveryCode);

            Guardar(new AdminCredenciales {
                PasswordHash= hash,
                PasswordSalt= salt,
                RecoveryCodeHash=recHash,
                RecoveryCodeSalt=recSalt,
                Configurado= true
            
            });
        }
        public bool ValidarPassword(string password)
        {
            var cred = Cargar();
            if (!cred.Configurado) return false;
            return VerificarValor(password, cred.PasswordHash, cred.PasswordSalt);
        }

        public bool ValidarRecoveryCode(string recoveryCode)
        {
            var cred = Cargar();
            if (!cred.Configurado) return false;
            return VerificarValor(recoveryCode, cred.RecoveryCodeHash, cred.RecoveryCodeSalt);
        }
        // Restablece la contraseña si el código de recuperación es correcto
        // Genera un nuevo código de recuperación (de un solo uso)

        public string RestablecerPassword(string recoveryCode, string nuevaPassword)
        {
            if (!ValidarRecoveryCode(recoveryCode))
                throw new InvalidOperationException("Codigo de recuperacion Invalido");

            var nuevoRecoveryCode = GenerarCodigoAleatorio();
            ConfigurarInicial(nuevaPassword, nuevoRecoveryCode);
            return nuevoRecoveryCode;
        }

        private static string GenerarCodigoAleatorio()
        {
            var bytes = RandomNumberGenerator.GetBytes(9);
            return Convert.ToBase64String(bytes).Replace("+", "").Replace("/", "").Replace("=", "");
        }

        private static (string hash, string salt) HashearValor(string valor)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(valor, saltBytes, 100_000, HashAlgorithmName.SHA256, 32);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        private static bool VerificarValor(string valor, string hashGuardado, string saltGuardado )
        {
            var satlBytes = Convert.FromBase64String(saltGuardado);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(valor, satlBytes, 100_000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(hashBytes, Convert.FromBase64String(hashGuardado));
        }

    }
}
