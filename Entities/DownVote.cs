using System;
using System.Collections.Generic;

namespace nyschub.Entities
{
    public class DownVote
    {
        public DownVote()
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