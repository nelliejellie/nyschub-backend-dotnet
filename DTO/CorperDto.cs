using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class CorperDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NyscRegNumber { get; set; }
        public string UserName { get; set; }

        public string JwtToken { get; set; }
    }
}
