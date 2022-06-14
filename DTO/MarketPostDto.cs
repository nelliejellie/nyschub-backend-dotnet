using Microsoft.AspNetCore.Http;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class MarketPostDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile> PhotoPaths { get; set; }
    }
}
