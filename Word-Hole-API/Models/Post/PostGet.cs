﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Models.Post
{
    public class PostGet
    {
        [Required]
        public int ID { get; set; }
    }
}
