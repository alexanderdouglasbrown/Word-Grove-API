﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Grove_API.Models.Like
{
    public class LikesDelete
    {
        [Required]
        public int PostID { get; set; }
    }
}
