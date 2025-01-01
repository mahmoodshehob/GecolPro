using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.DbEntity
{
    public class ReceiptsMessage
    {
        public int Id { get; set; }

        public string? ConversationId { get; set; }
        public string? TransactionId { get; set; }//Dcb

        public string? MSISDN { get; set; }

        public string? AmountGecol { get; set; }

        public string? AmountDCB { get; set; }
     
        public string? MessageContent { get; set; }  

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
