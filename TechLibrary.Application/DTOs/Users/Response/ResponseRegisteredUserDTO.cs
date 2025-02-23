using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLibrary.Application.DTOs.Users.Response
{
    public class ResponseRegisteredUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
