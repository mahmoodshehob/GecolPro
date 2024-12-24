namespace ZGecolPro.SmppClient.Models
{
    public class KannelStatus
    {


        public Boxconnection[] BoxConnections { get; set; }
        public Smscconnection[] SMSCConnections { get; set; }
    }

    public class Boxconnection
    {
        public string Type { get; set; }
        public Details Details { get; set; }
    }

    public class Details
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public int Queued { get; set; }
        public string OnlineTime { get; set; }
    }

    public class Smscconnection
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string Address { get; set; }
        public string Account { get; set; }
        public string ConnectionType { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        public int OnlineSeconds { get; set; }
        public Received Received { get; set; }
        public Sent Sent { get; set; }
        public int Failed { get; set; }
        public int Queued { get; set; }
    }

    public class Received
    {
        public Sms Sms { get; set; }
        public Dlr Dlr { get; set; }
    }

    public class Sms
    {
        public int Count { get; set; }
        public string[] Rate { get; set; }
    }

    public class Dlr
    {
        public int Count { get; set; }
        public string[] Rate { get; set; }
    }

    public class Sent
    {
        public Sms1 Sms { get; set; }
        public Dlr1 Dlr { get; set; }
    }

    public class Sms1
    {
        public int Count { get; set; }
        public string[] Rate { get; set; }
    }

    public class Dlr1
    {
        public int Count { get; set; }
        public string[] Rate { get; set; }
    }
}


