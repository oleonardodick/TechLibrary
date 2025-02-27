using System.Net;

namespace TechLibrary.Domain.Exceptions
{
    public class InvalidTokenException : TechLibraryException
    {
        public InvalidTokenException() : base("O token JWT não foi enviado ou é inválido.") {  }
        public override List<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}