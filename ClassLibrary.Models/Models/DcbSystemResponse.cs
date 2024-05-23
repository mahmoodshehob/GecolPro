using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Models
{
    public class DcbSystemResponse : ResponseService
    {
        public DcbSystemResponse(string response, string statusCode, bool isSuccessStatusCode) :
            base(response, statusCode, isSuccessStatusCode)
        {
        }
    }
}
