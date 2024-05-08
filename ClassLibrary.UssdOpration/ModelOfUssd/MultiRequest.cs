using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary.Models.ModelOfUssd
{
    public class MultiRequest
    {
        public string TransactionId { get; set; }
        public string TransactionTime { get; set; }
        public string MSISDN { get; set; }
        public string USSDServiceCode { get; set; }
        public string USSDRequestString { get; set; }
        public string response { get; set; }
    }
}