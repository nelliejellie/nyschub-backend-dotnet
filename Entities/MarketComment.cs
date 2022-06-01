using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class MarketComment
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public MarketPost MarketPost { get; set; }
        public DateTime CreatedAt { get; set; }
        public Corper Corper { get; set; }
        public List<UpVote> UpVotes { get; set; }
        public List<DownVote> DownVotes { get; set; }
    }
}
