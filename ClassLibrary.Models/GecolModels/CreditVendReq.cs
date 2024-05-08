using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ClassLibrary.Models.GecolModels
{
    public class CreditVendReq
    {

        public class Auth
        {
            public string EANDeviceID
            {
                get { return "0000000000001"; }
            }

            public string GenericDeviceID
            {
                get { return "0000000000001"; }
            }

            public string Username
            {
                get { return "AG0502"; }
            }

            public string Password
            {
                get { return "1234567891012"; }
            }
        }

        public class Parameters
        {
            public string DateTimeReq { set; get; }

            public string UniqueNumber { set; get; }

            [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
            public string MeterNumber { set; get; }

            [Range(3, int.MaxValue, ErrorMessage = "The Amount must be at least 3.")]
            public int Amount { set; get; }
        }
    }
}
