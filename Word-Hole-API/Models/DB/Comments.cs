using System;
using System.Collections.Generic;

namespace Word_Grove_API.Models.DB
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int Postid { get; set; }
        public string Comment { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime? Editdate { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
    }
}
