using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Entities
{
    public class ForumComment
    {
        public ForumComment()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string Post { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
