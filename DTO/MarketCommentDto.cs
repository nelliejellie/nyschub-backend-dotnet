using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class MarketCommentDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public MarketPost MarketPost { get; set; }
        public Corper Corper { get; set; }
        public List<UpVote> UpVotes { get; set; }
        public List<DownVote> DownVotes { get; set; }
    }
}
