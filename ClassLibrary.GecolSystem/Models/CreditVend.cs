using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ClassLibrary.GecolSystem.Models
{
    public class CreditVendReq : CommonParameters
    {

        [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
        public string MeterNumber { set; get; }

        [Range(3, int.MaxValue, ErrorMessage = "The Amount must be at least 3.")]
        public int PurchaseValue { set; get; }
    }



    //public class CreditVendRspXml
    //{

    //}

    //public class CreditVendRsp
    //{

    //}

}

