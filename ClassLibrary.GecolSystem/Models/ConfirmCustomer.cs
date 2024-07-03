using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.GecolSystem.Models
{
    public class ConfirmCustomerReq : CommonParameters
    {
        [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
        public string? MeterNumber { set; get; }
    }

   
}
