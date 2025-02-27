using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } 
            catch (TechLibraryException ex)
            {
                context.Response.StatusCode = (int)ex.GetStatusCode();
                await context.Response.WriteAsJsonAsync(new ResponseErrorMessagesDTO
                {
                    Errors = ex.GetErrorMessages()
                });
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ResponseErrorMessagesDTO
                {
                    Errors = ["Erro desconhecido."]
                });
            }
        }
    }
}
