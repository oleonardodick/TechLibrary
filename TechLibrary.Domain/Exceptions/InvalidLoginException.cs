using System.Net;

namespace TechLibrary.Domain.Exceptions
{
    public class InvalidLoginException : TechLibraryException
    {
        public InvalidLoginException() : base("E-mail e/ou senha inválidos.") { }

        public override List<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
