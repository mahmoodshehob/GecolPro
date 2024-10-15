using GecolPro.Main.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Text.Json;
using StackExchange.Redis;

namespace ConsoleApp_GecolSystem
{
    public class DeSerlV1
    {
        public string DoDeSerl(string xmlData)
        {
            // Parse the XML using XDocument
            XDocument xdoc = XDocument.Parse(xmlData);

            string responseSer ="" ;

            // Define namespaces to simplify queries
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ns2 = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema";
            XNamespace ns3 = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";


            CreditVendResp creditVendResp = new CreditVendResp();

            // Query the XML document
   
            creditVendResp.ClientID = xdoc.Descendants(ns2 + "clientID").FirstOrDefault()?.Attribute("ean")?.Value;
            
            creditVendResp.ServerID = xdoc.Descendants(ns2 + "serverID").FirstOrDefault()?.Attribute("ean")?.Value;
            
            creditVendResp.TerminalID = xdoc.Descendants(ns2 + "terminalID").FirstOrDefault()?.Attribute("id")?.Value;
            
            creditVendResp.ReqMsgID = new ReqMsgID()
            {
                Datetime = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("dateTime")?.Value,
                
                UniqueNumber = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("uniqueNumber")?.Value
            };

            creditVendResp.RespDateTime = xdoc.Descendants(ns2 + "respDateTime").FirstOrDefault()?.Value;

            creditVendResp.DispHeader = xdoc.Descendants(ns2 + "dispHeader").FirstOrDefault()?.Value;

            creditVendResp.ClientStatus = xdoc.Descendants(ns2 + "availCredit").FirstOrDefault()?.Attribute("value")?.Value;

            creditVendResp.CreditVendReceipt = xdoc.Descendants(ns3 + "creditVendReceipt").FirstOrDefault()?.Attribute("receiptNo")?.Value;

            creditVendResp.TenderAmount = xdoc.Descendants(ns3 + "tenderAmt").Attributes("value").First().Value;

            var transactions = xdoc.Descendants(ns3 + "tx").ToList();

            //////////////////////////
            ///
            /// Transactions / CreditVendTx
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
            ///  Transactions / CreditVendTx / Credit Token Issue
            ///  
            ///  Transactions / CreditVendTx / KC Token Issue
            ///
            //////////////////////////


            string KcTokenDesc = null;
            string KcTokenIssueSet1 = null;
            string KcTokenIssueSet2 = null;
            


            if (trans_CreditVendTx.Element(ns3 + "kcTokenIssue") != null)
            {
                KcTokenDesc = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "desc").Value;
                KcTokenIssueSet1 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set1stMeterKey").Element(ns2 + "stsCipher").Value;
                KcTokenIssueSet2 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set2ndMeterKey").Element(ns2 + "stsCipher").Value;
            }

            creditVendResp.CreditVendTx = new TransactionsCreditVendTx()
            {
                Amout = trans_CreditVendTx.Element(ns3 + "amt").Attribute("value")?.Value,
                Symbol = trans_CreditVendTx.Element(ns3 + "amt").Attribute("symbol")?.Value,
                Desc_CToken = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "desc").Value,
                STS1Token = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "token").Element(ns2 + "stsCipher").Value,
                Desc_KcToken= KcTokenDesc,
                Set1stMeterKey = KcTokenIssueSet1,
                Set2ndMeterKey= KcTokenIssueSet2,
                Tariff = trans_CreditVendTx.Element(ns3 + "tariff").Element(ns2 + "name").Value,
            };

            //////////////////////////
            ///
            ///  Transactions / ServiceChrgTx
            ///
            //////////////////////////


            if (trans_ServiceChrgTx != null)
            {
                creditVendResp.ServiceChrgTx = new List<TransactionsServiceChrgTx>();

                foreach (var trans_ServiceChrgTxp in trans_ServiceChrgTx)
                {

                    TransactionsServiceChrgTx serviceChrgTx = new TransactionsServiceChrgTx() 
                    {
                        AccDesc = trans_ServiceChrgTxp.Element(ns3 + "accDesc").Value,
                        Amout = trans_ServiceChrgTxp.Element(ns3 + "amt").Attribute("value")?.Value,
                        Tariff = trans_ServiceChrgTxp.Element(ns3 + "tariff").Value,

                    };

                    if (serviceChrgTx.AccDesc.Contains("everymonth * "))
                    {
                        var getNumOfMonth = serviceChrgTx.AccDesc.Split('*');

                        int NumOfMonthExpLastMonth = int.Parse(getNumOfMonth[1].Trim());

                        serviceChrgTx.Amout =   (int.Parse(serviceChrgTx.Amout) * NumOfMonthExpLastMonth).ToString();

                        serviceChrgTx.AccDesc = "Monthly Tax for old ("+NumOfMonthExpLastMonth+") months";
                    }

                    creditVendResp.ServiceChrgTx.Add(serviceChrgTx);

                   
                }
            }


            string creditVendRespJson = JsonSerializer.Serialize(creditVendResp);
            //Console.WriteLine(creditVendRespJson);

            responseSer += "\n";
            if (creditVendResp.CreditVendTx.Desc_KcToken != null)
            {
                responseSer += "\nNew Meter\n\n";
            }
            else
            {
                responseSer += "\nUsed Meter\n\n";
            }
            responseSer += "\n";


            if (creditVendResp.CreditVendTx.Desc_KcToken != null)
            {
                responseSer += "\n";

                responseSer += "Credit Token Issue Desc: " + creditVendResp.CreditVendTx.Desc_KcToken;
                responseSer += "\n\n";
                responseSer += "Set 1st Meter Key: " + creditVendResp.CreditVendTx.Set1stMeterKey;
                responseSer += "\n";
                responseSer += "Set 2nd Meter Key: " + creditVendResp.CreditVendTx.Set2ndMeterKey;
                responseSer += "\n\n";
            }



            responseSer += "Credit Token Issue Desc: " + creditVendResp.CreditVendTx.Desc_CToken;
            responseSer += "\n";
            responseSer += "Credit Vend Receipt: " + creditVendResp.CreditVendReceipt;
            responseSer += "\n";

            responseSer += "Total Transactions Amount: " + creditVendResp.TenderAmount + " LYD";
            responseSer += "\n";





            responseSer += "\n";

            responseSer += "Credit Token Issue Token: " + creditVendResp.CreditVendTx.STS1Token;
            responseSer += "\n";

            responseSer += "Transaction Amount: " + creditVendResp.CreditVendTx.Amout + " LYD";
            responseSer += "\n";


            foreach (var ser in creditVendResp.ServiceChrgTx)
            {

                responseSer += "\n\n\n";
                responseSer += "transaction Account Description: " + ser.AccDesc;
                responseSer += "\n\n";
                responseSer += "transaction Amount: " + ser.Amout + " LYD";

            }



            return responseSer;

        }
    }
}
