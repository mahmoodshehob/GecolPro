using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.Models
{

    public class FailureResponse
    {
        public string Failure { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}
