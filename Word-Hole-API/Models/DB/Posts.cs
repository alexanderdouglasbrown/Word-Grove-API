using System;
using System.Collections.Generic;

namespace Word_Grove_API.Models.DB
{
    public partial class Posts
    {
        public Posts()
        {
            Comments = new HashSet<Comments>();
            Likes = new HashSet<Likes>();
        }

        public int Id { get; set; }
        public int Userid { get; set; }
        public string Post { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime? Editdate { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Likes> Likes { get; set; }
    }
}
