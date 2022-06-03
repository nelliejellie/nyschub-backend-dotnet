﻿using Microsoft.AspNetCore.Http;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.DTO
{
    public class ForumPostDto
    {
        public string Post { get; set; }
        public string Caption { get; set; }
        public string Photo { get; set; }
    }
}
