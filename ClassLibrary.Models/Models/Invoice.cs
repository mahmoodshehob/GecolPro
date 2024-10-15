using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Models
{

    public class Invoice
    {
        public class CreditVendResp
        {
            public string? ClientID { set; get; }

            public string? ServerID { set; get; }

            public string? TerminalID { set; get; }

            public ReqMsgID ReqMsgID { set; get; }

            public string? RespDateTime { set; get; }

            public string? DispHeader { set; get; }

            public string? ClientStatus { set; get; }

            public string? CreditVendReceipt { set; get; }

            public TransactionsCreditVendTx? CreditVendTx { set; get; }

            public List<TransactionsServiceChrgTx>? ServiceChrgTx { set; get; }

            public string TenderAmount { set; get; }
        }

        public class ReqMsgID
        {
            public string Datetime { set; get; }
            public string UniqueNumber { set; get; }
        }

        public class TransactionsCreditVendTx
        {
            public string Amout { set; get; }

            public string Symbol { set; get; }

            public string? Desc_CToken { set; get; }

            public string STS1Token { set; get; }

            public string? Desc_KcToken { set; get; }

            public string? Set1stMeterKey { set; get; }

            public string? Set2ndMeterKey { set; get; }

            public string Tariff { set; get; }
        }

        public class TransactionsServiceChrgTx
        {
            public string Amout { set; get; }

            public string AccDesc { set; get; }

            public string Tariff { set; get; }
        }
    }

}
