namespace ZGecolPro.SmppClient.Models
{
    public class DlrMessage
    {
        public string Phone { get; set; } = "%p";
        public string msgId { get; set; }
        public string Status { get; set; } = "%d";
        public DateTime DeliveryDate { get; set; }

    }
}
