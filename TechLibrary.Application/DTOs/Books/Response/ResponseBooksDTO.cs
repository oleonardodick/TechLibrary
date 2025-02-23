using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLibrary.Application.DTOs.Pagination;

namespace TechLibrary.Application.DTOs.Books.Response
{
    public class ResponseBooksDTO
    {
        public PaginationDTO Pagination { get; set; } = default!;
        public List<ResponseBookDTO> Books { get; set; } = [];
    }
}
