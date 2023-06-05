using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Grove_API.Models.Post
{
    public class PostDelete
    {
        [Required]
        public int ID { get; set; }
    }
}
