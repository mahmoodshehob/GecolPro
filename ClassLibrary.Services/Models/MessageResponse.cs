using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Services.Models
{
    public class MessageResponse
    {
        public string Responce { get; set; }
        public int StatusCode { get; set; }
        public Boolean state { get; set; }
    }
}
