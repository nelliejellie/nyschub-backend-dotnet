using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class UpVote
    {
        public UpVote()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int ForumPostId { get; set; }
        public int ForumCommentId { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
