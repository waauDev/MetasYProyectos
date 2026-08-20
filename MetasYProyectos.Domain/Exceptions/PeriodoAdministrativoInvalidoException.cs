namespace MetasYProyectos.Domain.Exceptions
{
    public class PeriodoAdministrativoInvalidoException : Exception
    {
        public string Campo { get; }

        public PeriodoAdministrativoInvalidoException(string campo, string mensaje) : base(mensaje)
        {
            Campo = campo;
        }
    }
}
