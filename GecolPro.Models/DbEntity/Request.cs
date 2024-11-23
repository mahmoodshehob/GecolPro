namespace GecolPro.Models.DbEntity
{

    public class Request
    {
        public int Id { get; set; }
        public string? ConversationId { get; set; }
        
        public string? MSISDN{ get; set; }
        public string? Amount{ get; set; }
        public string? TotalTax { get; set; }

        public bool Status { get; set; }//true = Done , false = Failed

        public string? Token { get; set; }//Gecol
        public string? TransactionId { get; set; }//Dcb


        public string? MeterNumber { get; set; }        
        public string? UniqueNumber { get; set; }

        public string FromSystem { get; set; } //Gecol Or Dcb

        public DateTime CreatedDate { get; set; }= DateTime.Now;

        public string[] Tokens { get { return Token.Split(";"); } }

    }
}
