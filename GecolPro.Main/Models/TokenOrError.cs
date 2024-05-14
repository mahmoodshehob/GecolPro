using System.ComponentModel.DataAnnotations;

namespace GecolPro.Main.Models
{
    public class TokenOrError
    {

        [Required]
        public string? TknOrErr{ get; set; }

        [Required]
        public Boolean Status { get; set; }

    }
}
