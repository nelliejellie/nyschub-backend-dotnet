using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class MarketPostDto
    {
        public string PhotoPath { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Corper Corper { get; set; }
        public List<MarketComment> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
