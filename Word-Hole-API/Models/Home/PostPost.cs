using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API.Models.Home
{
    public class PostPost
    {
        [Required(AllowEmptyStrings = false)]
        public string Post { get; set; }
    }
}
