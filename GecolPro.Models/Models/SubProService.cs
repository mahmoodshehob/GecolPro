using System.ComponentModel.DataAnnotations;


namespace  GecolPro.Models.Models
{
    public class SubProService
    {
        public SubProService() 
        {
            Random random = new Random();
            UniqueNumber = random.Next(1, 999999).ToString("D6");

        }
        private Random random = new Random();


        [Required]
        public string ConversationID { get; set; }


        //This in case billing success
        public string TransactionID { get; set; }

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
    }
}
