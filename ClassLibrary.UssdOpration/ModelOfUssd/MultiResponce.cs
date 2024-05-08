using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary.Models.ModelOfUssd
{
    public class MultiResponce
    {
        public string TransactionId { get; set; }
        public string TransactionTime { get; set; }
        public string USSDResponseString { get; set; }
        public string MSISDN { get; set; }
        public string Action { get; set; }

    }
}