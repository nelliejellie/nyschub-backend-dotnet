using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class MarketPost
    {
        public MarketPost()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public bool IsSold = false;
        public int Id { get; set; }
        [Required]
        public string PhotoPath { get; set; }
        [Required]
        public string PhotoPathTwo { get; set; }
        [Required]
        public string PhotoPathThree { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public string StatePost { get; set; }
        public string CorperName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
