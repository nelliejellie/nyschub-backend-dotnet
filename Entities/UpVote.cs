using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class UpVote
    {
        public int Id { get; set; }
        public ForumPost ForumPost { get; set; }
        public ForumComment ForumComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Corper CorperAgainst { get; set; }
    }
}
