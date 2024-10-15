using GecolPro.Main.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace GecolPro.Main.Test
{

    public class DeSerl
    {
        public void DoDeSerl(string xmlData)
        {
            // Parse the XML using XDocument
            XDocument xdoc = XDocument.Parse(xmlData);

            // Define namespaces to simplify queries
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ns2 = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema";
            XNamespace ns3 = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";


            CreditVendResp creditVendResp = new CreditVendResp();

                        // Query the XML document



                        var clientIDEan = xdoc.Descendants(ns2 + "clientID").FirstOrDefault()?.Attribute("ean")?.Value;

            var serverIDEan = xdoc.Descendants(ns2 + "serverID").FirstOrDefault()?.Attribute("ean")?.Value;

            var terminalIDid  = xdoc.Descendants(ns2 + "terminalID").FirstOrDefault()?.Attribute("id")?.Value;

            var reqMsgID_dateTime = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("dateTime")?.Value;

            var reqMsgID_uniqueNumber = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("uniqueNumber")?.Value;

            /////////////////////////////


            var availCredit = xdoc.Descendants(ns2 + "availCredit").FirstOrDefault()?.Attribute("value")?.Value;



            var creditVendReceipt = xdoc.Descendants(ns3 + "creditVendReceipt").FirstOrDefault()?.Attribute("receiptNo")?.Value;

            var transactions = xdoc.Descendants(ns3 + "tx").ToList();
            var transactions_tenderAmt = xdoc.Descendants(ns3 + "tenderAmt").Attributes("value").First().Value;

            //////////////////////////
            ///
            /// Transactions creditVendReceipt
            ///
            //////////////////////////

            var trans_CreditVendTx = transactions[0];


            List<XElement> trans_ServiceChrgTx = new List<XElement>();
            for (int i = 1; i < transactions.Count; i++)
            {
                trans_ServiceChrgTx.Add(transactions[i]);
            }

            //////////////////////////
            ///
            /// Transactions / CreditVendTx
            ///
            //////////////////////////

            //var transactions_amt = transactions.FirstOrDefault()?.Element(ns3 + "amt").Attribute("value")?.Value;
            //var transactions_symbol = transactions.FirstOrDefault()?.Element(ns3 + "amt").Attribute("symbol")?.Value;

            var trans_amt = trans_CreditVendTx.Element(ns3 + "amt").Attribute("value")?.Value;
            var trans_symbol = trans_CreditVendTx.Element(ns3 + "amt").Attribute("symbol")?.Value;
            var trans_CreditVendTx_creditTokenIssue = trans_CreditVendTx.Element(ns3 + "creditTokenIssue");
            var trans_CreditVendTx_kcTokenIssue = trans_CreditVendTx.Element(ns3 + "kcTokenIssue");
            var trans_CreditVendTx_tariff = trans_CreditVendTx.Element(ns3 + "tariff");

            //////////////////////////
            ///
            ///  Transactions / CreditVendTx / Credit Token Issue
            ///
            //////////////////////////

            var SaleCredTokenIssue = trans_CreditVendTx_creditTokenIssue.Element(ns2 + "token").Element(ns2 + "stsCipher").Value;

            //////////////////////////
            ///
            ///  Transactions / CreditVendTx / KC Token Issue
            ///
            //////////////////////////

            string KcTokenIssueSet1;
            string KcTokenIssueSet2;

            if (trans_CreditVendTx_kcTokenIssue != null)
            {
                KcTokenIssueSet1 = trans_CreditVendTx_kcTokenIssue.Element(ns2 + "token").Element(ns2 + "set1stMeterKey").Element(ns2 + "stsCipher").Value;
                KcTokenIssueSet2 = trans_CreditVendTx_kcTokenIssue.Element(ns2 + "token").Element(ns2 + "set2ndMeterKey").Element(ns2 + "stsCipher").Value;

            }

            //////////////////////////
            ///
            ///  Transactions / CreditVendTx / Tariff
            ///
            //////////////////////////

            string trans_CreditVendTx_tariffName;

            if (trans_CreditVendTx_tariff != null)
            {
                trans_CreditVendTx_tariffName = trans_CreditVendTx_tariff.Element(ns2 + "name").Value;
            }

            //////////////////////////
            ///
            ///  Transactions / ServiceChrgTx
            ///
            //////////////////////////

            List<Dictionary<string, string>> ServiceChrgTx = new List<Dictionary<string,string>>();

            if (trans_ServiceChrgTx != null)
            {
                foreach (var trans_ServiceChrgTxp in trans_ServiceChrgTx)
                {
                    string amt = trans_ServiceChrgTxp.Element(ns3 + "amt").Attribute("value")?.Value;
                    string accDesc = trans_ServiceChrgTxp.Element(ns3 + "accDesc").Value;
                    ServiceChrgTx.Add(new Dictionary<string, string> {{accDesc, amt}});
                }
            }


            var creditTokenIssue = transactions.FirstOrDefault()?.Element(ns3 + "creditTokenIssue");
            var creditTokenIssue_desc = creditTokenIssue.Element(ns2 + "desc").Value;
            var creditTokenIssue_token = creditTokenIssue.Element(ns2 + "token").Value;

            var kcTokenIssue = transactions.FirstOrDefault()?.Element(ns3 + "kcTokenIssue");


            var firstTransactionAmount = transactions.FirstOrDefault()?
                                        .Element(ns3 + "amt")?
                                        .Attribute("value")?.Value;






            //var STS1Token = transactions.Descendants(ns2 + "token").ToList(); 

            //var stsCipher = STS1Token.FirstOrDefault()?.Value;

            //var KCToken = transactions.Descendants(ns2 + "token");

            Console.WriteLine("Credit Vend Receipt: " + creditVendReceipt);
            Console.WriteLine("Total Transactions Amount: " + transactions_tenderAmt + " LYD");
            Console.WriteLine("Credit Token Issue Desc: " + creditTokenIssue_desc);
            
            if (kcTokenIssue != null)
            {
                Console.WriteLine("\n");
                string set1stMeterKey = kcTokenIssue.Element(ns2 + "token").Element(ns2 + "set1stMeterKey").Element(ns2 + "stsCipher").Value;
                string set2ndMeterKey = kcTokenIssue.Element(ns2 + "token").Element(ns2 + "set2ndMeterKey").Element(ns2 + "stsCipher").Value;
                Console.WriteLine("Set 1st Meter Key: " + set1stMeterKey);
                Console.WriteLine("Set 2nd Meter Key: " + set2ndMeterKey);
            }


            Console.WriteLine("\n");

            Console.WriteLine("Credit Token Issue Token: " + creditTokenIssue_token);

            Console.WriteLine("Transaction Amount: " + firstTransactionAmount + " LYD");


            foreach (var transaction in transactions)
            {
                if (transaction.Attribute(xsi + "type").Value == "ns3:ServiceChrgTx")
                {
                    var transaction_Amount = (decimal)transaction.Element(ns3 + "amt")?.Attribute("value");
                    var transaction_AccountDescription = (string)transaction.Element(ns3 + "accDesc");
                    var transaction_AccountNumber = (string)transaction.Element(ns3 + "accNo");
                    var transaction_TariffName = (string)transaction.Element(ns3 + "tariff")?.Element(ns3 + "name");

                    Console.WriteLine("\n");
                    Console.WriteLine("transaction Account Description: " + transaction_AccountDescription);

                    Console.WriteLine("transaction Amount: " + transaction_Amount + " LYD");
                }
            }

        }
    }
}
