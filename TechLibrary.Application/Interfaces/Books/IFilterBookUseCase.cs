using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Application.DTOs.Books.Response;

namespace TechLibrary.Application.Interfaces.Books
{
    public interface IFilterBooksUseCase
    {
        Task<ResponseBooksDTO> ExecuteAsync(RequestFilterBooksDTO request);
    }
}
