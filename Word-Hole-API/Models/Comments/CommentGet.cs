using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Models.Comments
{
    public class CommentGet
    {
        [Required]
        public int CommentID { get; set; }
    }
}
