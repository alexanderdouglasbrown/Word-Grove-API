using System;
using System.Collections.Generic;

namespace Word_Grove_API.Models.DB
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            Likes = new HashSet<Likes>();
            Posts = new HashSet<Posts>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public DateTime Createdon { get; set; }
        public string Access { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Likes> Likes { get; set; }
        public virtual ICollection<Posts> Posts { get; set; }
    }
}
