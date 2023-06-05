using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Grove_API.Models.Settings
{
    public class UpdateProfilePatch
    {
        [Required(AllowEmptyStrings = false)]
        public string Current { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string New { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Confirm { get; set; }
    }
}
