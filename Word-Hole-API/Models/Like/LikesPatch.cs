using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Models.Like
{
    public class LikesPatch
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int PostID { get; set; }
    }
}
