using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess.Models
{
    public class IssueTkn
    {
        public int Id { get; set; }
        [Required]
        public string ConversationID { get; set; }

        [Required]
        public string MSISDN { get; set; }

        [Required]
        public string DateTimeReq { set; get; }   // this maybe not needed

        [Required]
        public string UniqueNumber { set; get; }

        [Required]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
        public string MeterNumber { set; get; }

        [Required]
        [Range(3, int.MaxValue, ErrorMessage = "The Amount must be at least 3.")]
        public int Amount { set; get; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
