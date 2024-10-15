namespace GecolPro.DCBSystem.Models
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
                get => _conversationID;
                set => _conversationID = value;
            }

            public string TransactionID
            {
                get => _transactionID;
                set => _transactionID = value;
            }

            public string ServiceName
            {
                get => _service;
                set => _service = value;
            }

            public string ProviderName
            {
                get => _service;
                set => _service = value;
            }


            public string OriginatingAddress
            {
                get => _msisdn;
                set => _msisdn = value;
            }

            public string DestinationAddress
            {
                get => _msisdn;
                set => _msisdn = value;
            }

            public string ChargingAddress
            {
                get => _msisdn;
                set => _msisdn = value;
            }

            public int Amount
            {
                get => _amount;
                set => _amount = value;
            }
        }

        public  class DebitObjResp
        {
            public string ConversationID { get; set; }
            public string TransactionID { get; set; }
            public string Amount { get; set; }
        }
    }
}
