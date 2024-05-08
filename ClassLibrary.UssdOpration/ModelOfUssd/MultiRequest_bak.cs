using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary.Models.ModelOfUssd
{
    public class MultiRequest_bak
    {
        public ReqElement TransactionId { get; set; }
        public ReqElement TransactionTime { get; set; }
        public ReqElement MSISDN { get; set; }
        public ReqElement USSDServiceCode { get; set; }
        public ReqElement USSDRequestString { get; set; }
        public ReqElement response { get; set; }
     }
}