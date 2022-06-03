using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class ForumPost
    {
        public ForumPost()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public string Post { get; set; }
        public string Caption { get; set; }
        public List<ForumComment> Comments { get; set; }
        public Corper Corper { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UpVote> UpVotes { get; set; }
        public List<DownVote> DownVotes { get; set; }
    }
}
