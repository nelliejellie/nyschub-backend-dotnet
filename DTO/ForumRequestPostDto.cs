using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class ForumRequestPostDto
    {
        public string Post { get; set; }
        public string Caption { get; set; }
        public IFormFile PhotoPath { get; set; }
    }
}

