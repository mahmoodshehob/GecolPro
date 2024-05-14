using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GecolPro.Main.Models
{
    public class MultiResponseUSSD
    {
        public string? TransactionId { get; set; }
        public string? TransactionTime { get; set; }
        public string? MSISDN { get; set; }
        public string? USSDServiceCode { get; set; }
        public string? USSDResponseString { get; set; }
        public string? Action { get; set; }
        public int? ResponseCode { get; set; }
    }
}