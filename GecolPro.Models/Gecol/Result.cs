using GecolPro.Models.Gecol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.Gecol
{
    public class SuccessResponseLogin
    {
        public LoginRspXml.LoginRsp Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }

    public class SuccessResponseConfirmCustomer
    {
        public ConfirmCustomerRespBody Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }

    public class SuccessResponseCreditVend
    {
        public CreditVendRespBody.CreditVendResp? Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }

}
