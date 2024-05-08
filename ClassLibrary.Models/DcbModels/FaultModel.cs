using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.DcbModels
{
    public class FaultModel
    {
        public string FaultCode { get; set; }
        public string FaultString { get; set; }
        public string Detail { get; set; }
    }
}