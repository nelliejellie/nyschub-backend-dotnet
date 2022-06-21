using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
    }
}
