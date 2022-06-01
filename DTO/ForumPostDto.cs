using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class ForumPostDto
    {
        public string PhotoPath { get; set; }
        public string Post { get; set; }
        public string Caption { get; set; }
        public List<ForumComment> Comments { get; set; }
        public Corper Corper { get; set; }
        public List<UpVote> UpVotes { get; set; }
        public List<DownVote> DownVotes { get; set; }
    }
}
