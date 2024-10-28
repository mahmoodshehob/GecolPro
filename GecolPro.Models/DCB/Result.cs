using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.DCB
{
    public class SuccessResponseQryUserBasicBal
    {
        public QryUserBasicBalRsp Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }

    public class SuccessResponseDirectDebit
    {
        public DirectDebitUnitRsp Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }

}
