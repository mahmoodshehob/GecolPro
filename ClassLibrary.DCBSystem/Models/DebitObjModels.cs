using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DCBSystem.Models
{
    public class DebitObjModels
    {

        public class DebitObjReq
        {
            private string _conversationID ;
            private string _transactionID;
            private string _service;
            private string _msisdn;
            private int _amount;



            public string ConversationID
            {
                get { return _conversationID; }
                set { _conversationID = value; }
            }

            public string TransactionID
            {
                get { return _transactionID; }
                set { _transactionID = value; }
            }

            public string ServiceName
            {
                get { return _service; }
                set { _service = value; }
            }

            public string ProviderName
            {
                get { return _service; }
                set { _service = value; }
            }


            public string OriginatingAddress
            {
                get { return _msisdn; }
                set { _msisdn = value; }
            }

            public string DestinationAddress
            {
                get { return _msisdn; }
                set { _msisdn = value; }
            }

            public string ChargingAddress
            {
                get { return _msisdn; }
                set { _msisdn = value; }
            }

            public int Amount
            {
                get { return _amount; }
                set { _amount = value; }
            }
        }

        public partial class DebitObjResp
        {
            public string ConversationID { get; set; }
            public string TransactionID { get; set; }
            public string Amount { get; set; }
        }
    }
}
