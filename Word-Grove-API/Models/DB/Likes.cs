﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace Word_Grove_API.Models.DB
{
    public partial class Likes
    {
        public int Userid { get; set; }
        public int Postid { get; set; }
        public BitArray Hax { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
    }
}
