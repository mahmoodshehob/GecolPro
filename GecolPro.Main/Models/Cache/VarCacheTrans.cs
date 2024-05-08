using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GecolPro.Main.Models.Cache
{

    public class VarCacheTrans
    {
        public string MSISDN { get; set; }
        public string? MenuIdNew { get; set; }
        public string Input { get; set; }
        public string ServiceName { get; set; }
        public Dictionary<string,string>? Para { get; set; }
    }
}