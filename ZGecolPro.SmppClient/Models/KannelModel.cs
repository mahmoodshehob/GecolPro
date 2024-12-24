

namespace ZGecolPro.SmppClient.Models
{
    public class KannelModel
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DlrEndPoint { get; set; }
        public int? DlrMask { get; set; } = 7;

    }
}
