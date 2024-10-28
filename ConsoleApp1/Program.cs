using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GecolPro.Models.Gecol;
using static GecolPro.Models.Gecol.CreditVendRespBody;



namespace ConsoleApp1
{
    internal class Program
    {

        private static readonly XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";

        private static readonly XNamespace ns2 = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema";

        private static readonly XNamespace ns3 = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema";

        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";


        static void Main(string[] args)
        {
            string files = System.IO.File.ReadAllText(@"D:\OneDrive\Gecol\xml api\confirmCustomerRsp.xml");

            ConfirmCustomerRespBody confirmCustomerRespBody = new ConfirmCustomerRespBody();

            confirmCustomerRespBody = ToCreateXmlCustomerCRsp(files);

            Console.WriteLine("Hello, World!");
        }

        public static ConfirmCustomerRespBody ToCreateXmlCustomerCRsp(string xmlSoapResponse)
        {
            // Parse the XML using XDocument
            XDocument xdoc = XDocument.Parse(xmlSoapResponse);

            ConfirmCustomerRespBody ConfCustRespBody = new ConfirmCustomerRespBody();

            // Query the XML document

            var confirmCustResult = xdoc.Descendants(ns3 + "meterDetail").FirstOrDefault();
            string meterTypeAt = confirmCustResult.Element(ns2 + "meterType")?.Attribute("at")?.Value;
            string meterTypeTt = confirmCustResult.Element(ns2 + "meterType")?.Attribute("tt")?.Value;

            return null;
            //////////////////////////
            ///
            /// Transactions / CreditVendTx
            ///
            //////////////////////////
            //XElement trans_CreditVendTx = transactions[0];

            //XElement? trans_RecoveryTx = null;

            //List<XElement> trans_ServiceChrgTx = new List<XElement>();


            //for (int i = 1; i < transactions.Count; i++)
            //{
            //    string _attrib = transactions[i].Attribute(xsi + "type")?.Value;

            //    if (_attrib == "ns3:DebtRecoveryTx")
            //    {

            //        trans_RecoveryTx = transactions[i];
            //    }
            //    if (_attrib == "ns3:ServiceChrgTx")
            //    {
            //        trans_ServiceChrgTx.Add(transactions[i]);
            //    }
            //}

            ////////////////////////////
            /////
            /////  Transactions / CreditVendTx / Credit Token Issue
            /////  
            /////  Transactions / CreditVendTx / KC Token Issue
            /////
            ////////////////////////////


            //string KcTokenDesc = null;
            //string KcTokenIssueSet1 = null;
            //string KcTokenIssueSet2 = null;



            //if (trans_CreditVendTx.Element(ns3 + "kcTokenIssue") != null)
            //{
            //    KcTokenDesc = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "desc").Value;
            //    KcTokenIssueSet1 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set1stMeterKey").Element(ns2 + "stsCipher").Value;
            //    KcTokenIssueSet2 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set2ndMeterKey").Element(ns2 + "stsCipher").Value;
            //}

            //creditVendResp.CreditVendTx = new TransactionsCreditVendTx()
            //{
            //    Amout = trans_CreditVendTx.Element(ns3 + "amt").Attribute("value")?.Value,
            //    Symbol = trans_CreditVendTx.Element(ns3 + "amt").Attribute("symbol")?.Value,
            //    Desc_CToken = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "desc").Value,
            //    STS1Token = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "token").Element(ns2 + "stsCipher").Value,
            //    Desc_KcToken = KcTokenDesc,
            //    Set1stMeterKey = KcTokenIssueSet1,
            //    Set2ndMeterKey = KcTokenIssueSet2,
            //    Tariff = trans_CreditVendTx.Element(ns3 + "tariff").Element(ns2 + "name").Value,
            //};




            ////////////////////////////
            /////
            /////  Transactions / RecoveryTx
            /////
            ////////////////////////////


            //if (trans_RecoveryTx != null)
            //{
            //    creditVendResp.RecoveryTx = new TransactionsDebtRecoveryTx()
            //    {
            //        AccDesc = trans_RecoveryTx.Element(ns3 + "accDesc").Value,
            //        Amout = trans_RecoveryTx.Element(ns3 + "amt").Attribute("value")?.Value,
            //        Tariff = trans_RecoveryTx.Element(ns3 + "tariff").Value,
            //        Balance = trans_RecoveryTx.Element(ns3 + "balance").Attribute("value")?.Value

            //    };
            //}



            ////////////////////////////
            /////
            /////  Transactions / ServiceChrgTx
            /////
            ////////////////////////////


            //if (trans_ServiceChrgTx == null || trans_ServiceChrgTx.Count == 0)
            //{
            //    creditVendResp.ServiceChrgTx = null;
            //}
            //else if (trans_ServiceChrgTx != null)
            //{
            //    creditVendResp.ServiceChrgTx = new List<TransactionsServiceChrgTx>();

            //    foreach (var trans_ServiceChrgTxp in trans_ServiceChrgTx)
            //    {

            //        TransactionsServiceChrgTx serviceChrgTx = new TransactionsServiceChrgTx()
            //        {
            //            AccDesc = trans_ServiceChrgTxp.Element(ns3 + "accDesc").Value,
            //            Amout = trans_ServiceChrgTxp.Element(ns3 + "amt").Attribute("value")?.Value,
            //            Tariff = trans_ServiceChrgTxp.Element(ns3 + "tariff").Value,

            //        };


            //        if (serviceChrgTx.AccDesc.Contains("everymonth * "))
            //        {
            //            var getNumOfMonth = serviceChrgTx.AccDesc.Split('*');

            //            int NumOfMonthExpLastMonth = int.Parse(getNumOfMonth[1].Trim());

            //            serviceChrgTx.Amout = (int.Parse(serviceChrgTx.Amout) * NumOfMonthExpLastMonth).ToString();

            //            serviceChrgTx.AccDesc = "Monthly Tax for old (" + NumOfMonthExpLastMonth + ") months";
            //        }

            //        creditVendResp.ServiceChrgTx.Add(serviceChrgTx);
            //    }
            //}


            return null;

        }

    }
}