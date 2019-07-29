using System;
using System.Collections.Generic;

namespace Word_Hole_API.Models.DB
{
    public partial class Users
    {
        public Users()
        {
            Posts = new HashSet<Posts>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public DateTime Createdon { get; set; }
        public string Access { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }
    }
}
