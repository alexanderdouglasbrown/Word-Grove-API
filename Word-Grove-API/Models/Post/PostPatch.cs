using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Grove_API.Models.Post
{
    public class PostPatch
    {
        [Required]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Post { get; set; }

        public string ImageURL { get; set; }
    }
}
