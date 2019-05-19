using System;
using System.Collections.Generic;

namespace Word_Hole_API.Models
{
    public partial class Posts
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Message { get; set; }
        public DateTime Createdon { get; set; }

        public virtual Users User { get; set; }
    }
}
