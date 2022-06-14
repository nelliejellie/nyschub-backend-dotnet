using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class MarketComment
    {
        public MarketComment()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public string UserName { get; set; }

        public int MarketPostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
