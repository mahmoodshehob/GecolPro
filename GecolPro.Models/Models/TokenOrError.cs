using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.Models
{
    public class TokenOrError
    {

        [Required]
        public string? TknOrErr { get; set; }

        [Required]
        public Boolean Status { get; set; }

    }
}
