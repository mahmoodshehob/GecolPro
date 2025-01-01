namespace GecolPro.Models.Models
{
    public class SmsMessage
    {
        public string? msgID { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public string? Profile { get; set; }

    }
}
