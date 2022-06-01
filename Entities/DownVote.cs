using System;
using System.Collections.Generic;

namespace nyschub.Entities
{
    public class DownVote
    {
        public int Id { get; set; }
        public ForumPost ForumPost{get; set;}
        public ForumComment ForumComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Corper CorperAgainst { get; set; }
    }
}