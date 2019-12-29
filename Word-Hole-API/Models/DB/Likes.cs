using System;
using System.Collections.Generic;

namespace Word_Hole_API.Models.DB
{
    public partial class Likes
    {
        public int Userid { get; set; }
        public int Postid { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
    }
}
