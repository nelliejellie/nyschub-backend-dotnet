﻿using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class UpVoteDto
    {
        public int ForumPostId { get; set; }
        public string UserName { get; set; }
    }
}
