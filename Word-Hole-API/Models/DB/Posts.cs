using System;
using System.Collections.Generic;

namespace Word_Hole_API.Models.DB
{
    public partial class Posts
    {
        public int Id { get; set; }
        public int? Userid { get; set; }
        public string Post { get; set; }
        public DateTime Createdon { get; set; }

        public virtual Users User { get; set; }
    }
}
