using System.ComponentModel.DataAnnotations;

namespace GecolPro.Models.Gecol
{
    public class ConfirmCustomerReq : CommonParameters
    {
        [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
        public string? MeterNumber { set; get; }
    }


    public class ConfirmCustomerRespBody
    {
        
        public string? CustomerNumber { set; get; }
        public string? AT { set; get; }
        public string? TT { set; get; }
        public float? MaxVendAmt { set; get; }
        public float? MinVendAmt { set; get; }
        public CustVendDetail? CustVendDetail { set; get; }





    }

    public class CustVendDetail()
    {
        public string? AccNo { set; get; }
        public string? Address { set; get; }
        public string? ContactNo { set; get; }
        public string? DaysLastPurchase { set; get; }
        public string? LocRef { set; get; }
        public string? Name { set; get; }
    }


}


