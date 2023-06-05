using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Grove_API.Models.Comments
{
    public class CommentEditPatch
    {
        [Required]
        public int CommentID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Comment { get; set; }
    }
}
