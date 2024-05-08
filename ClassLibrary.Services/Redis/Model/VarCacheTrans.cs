using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Services.Redis.Model
{
    public class VarCacheTrans
    {
        public string MSISDN { get; set; }
        public string? MenuIdNew { get; set; }
        public string Input { get; set; }
        public string ServiceName { get; set; }
        public Dictionary<string, string>? Para { get; set; }
    }
}
