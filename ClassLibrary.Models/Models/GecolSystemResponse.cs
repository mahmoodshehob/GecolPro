using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Models
{
    public class GecolSystemResponse : ResponseService
    {
        public GecolSystemResponse(string response, string statusCode, bool isSuccessStatusCode) : 
            base(response, statusCode, isSuccessStatusCode)
        {
        }
    }
}
