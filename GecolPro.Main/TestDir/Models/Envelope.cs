//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace ConsoleApp_GecolSystem.Models
//{


//    using System;
//    using System.Collections.Generic;

//    public class Envelope
//    {
//        public Header Header { get; set; }
//        public Body Body { get; set; }
//    }

//    public class Header { }

//    public class Body
//    {
//        public CreditVendResp CreditVendResp { get; set; }
//    }

//    public class CreditVendResp
//    {
//        public ClientID ClientID { get; set; }
//        public ServerID ServerID { get; set; }
//        public TerminalID TerminalID { get; set; }
//        public ReqMsgID ReqMsgID { get; set; }
//        public DateTime RespDateTime { get; set; }
//        public string DispHeader { get; set; }
//        public ClientStatus ClientStatus { get; set; }
//        public Utility Utility { get; set; }
//        public Vendor Vendor { get; set; }
//        public CustVendDetail CustVendDetail { get; set; }
//        public CreditVendReceipt CreditVendReceipt { get; set; }
//    }

//    public class ClientID
//    {
//        public string Ean { get; set; }
//        public string Type { get; set; }
//    }

//    public class ServerID
//    {
//        public string Ean { get; set; }
//        public string Type { get; set; }
//    }

//    public class TerminalID
//    {
//        public string Id { get; set; }
//        public string Type { get; set; }
//    }

//    public class ReqMsgID
//    {
//        public string DateTime { get; set; }
//        public string UniqueNumber { get; set; }
//    }

//    public class ClientStatus
//    {
//        public AvailCredit AvailCredit { get; set; }
//    }

//    public class AvailCredit
//    {
//        public string Symbol { get; set; }
//        public decimal Value { get; set; }
//    }

//    public class Utility
//    {
//        public string Address { get; set; }
//        public string Name { get; set; }
//    }

//    public class Vendor
//    {
//        public string Address { get; set; }
//        public string Name { get; set; }
//    }

//    public class CustVendDetail
//    {
//        public string AccNo { get; set; }
//        public string Address { get; set; }
//        public int DaysLastPurchase { get; set; }
//        public string LocRef { get; set; }
//        public string Name { get; set; }
//    }

//    public class CreditVendReceipt
//    {
//        public string ReceiptNo { get; set; }
//        public Transactions Transactions { get; set; }
//    }

//    public class Transactions
//    {
//        public List<Tx> TxList { get; set; }
//        public Amount LessRound { get; set; }
//        public Amount TenderAmt { get; set; }
//        public Amount Change { get; set; }
//    }

//    public class Tx
//    {
//        public string ReceiptNo { get; set; }
//        public string Type { get; set; }
//        public Amount Amt { get; set; }
//        public CreditTokenIssue CreditTokenIssue { get; set; }
//        public KCTokenIssue KCTokenIssue { get; set; }
//        public string AccDesc { get; set; }
//        public string AccNo { get; set; }
//        public Tariff Tariff { get; set; }
//    }

//    public class Amount
//    {
//        public string Symbol { get; set; }
//        public decimal Value { get; set; }
//    }

//    public class CreditTokenIssue
//    {
//        public string Type { get; set; }
//        public string Desc { get; set; }
//        public MeterDetail MeterDetail { get; set; }
//        public Token Token { get; set; }
//        public Units Units { get; set; }
//        public Resource Resource { get; set; }
//    }

//    public class KCTokenIssue
//    {
//        public string Desc { get; set; }
//        public MeterDetail MeterDetail { get; set; }
//        public KCToken Token { get; set; }
//        public KCTData KctData { get; set; }
//    }

//    public class MeterDetail
//    {
//        public string Krn { get; set; }
//        public string Msno { get; set; }
//        public string Sgc { get; set; }
//        public string Ti { get; set; }
//        public MeterType MeterType { get; set; }
//        public string MaxVendAmt { get; set; }
//        public string MinVendAmt { get; set; }
//        public string MaxVendEng { get; set; }
//        public string MinVendEng { get; set; }
//    }

//    public class MeterType
//    {
//        public string At { get; set; }
//        public string Tt { get; set; }
//    }

//    public class Token
//    {
//        public string StsCipher { get; set; }
//    }

//    public class Units
//    {
//        public string SiUnit { get; set; }
//        public decimal Value { get; set; }
//    }

//    public class Resource
//    {
//        public string Type { get; set; }
//    }

//    public class KCToken
//    {
//        public SetMeterKey Set1stMeterKey { get; set; }
//        public SetMeterKey Set2ndMeterKey { get; set; }
//    }

//    public class SetMeterKey
//    {
//        public string StsCipher { get; set; }
//    }

//    public class KCTData
//    {
//        public string FromKRN { get; set; }
//        public string FromSGC { get; set; }
//        public string FromTI { get; set; }
//        public string ToKRN { get; set; }
//        public string ToSGC { get; set; }
//        public string ToTI { get; set; }
//    }

//    public class Tariff
//    {
//        public string Name { get; set; }
//    }
//}
