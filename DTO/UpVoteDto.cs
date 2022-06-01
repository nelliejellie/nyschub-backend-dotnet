using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class UpVoteDto
    {
        public ForumPost ForumPost { get; set; }
        public ForumComment ForumComment { get; set; }
        public Corper CorperAgainst { get; set; }
    }
}
