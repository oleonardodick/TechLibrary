using System.Net;

namespace TechLibrary.Exception
{
    public class InvalidLoginException : TechLibraryException
    {
        public override List<string> GetErrorMessages() => ["E-mail e/ou senha inválidos"];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
