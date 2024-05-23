using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Models
{
    public class ResponseService
    {
        public ResponseService(string response, string statusCode, bool isSuccessStatusCode)
        {
            Response = response;
            StatusCode = statusCode;
            IsSuccessStatusCode = isSuccessStatusCode;
        }

        public string Response { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}
