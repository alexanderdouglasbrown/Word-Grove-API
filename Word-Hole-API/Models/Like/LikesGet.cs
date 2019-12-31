using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Models.Like
{
    public class LikesGet
    {
        [Required]
        public int PostID { get; set; }
    }
}
