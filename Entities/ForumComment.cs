using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class ForumComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public ForumPost ForumPost { get; set; }
        public DateTime CreatedAt { get; set; }
        public Corper Corper { get; set; }
        public List<UpVote> UpVotes { get; set; }
        public List<DownVote> DownVotes { get; set; }
    }
}
