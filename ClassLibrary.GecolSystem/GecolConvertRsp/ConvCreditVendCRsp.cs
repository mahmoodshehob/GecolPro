using ClassLibrary.GecolSystem.Models;
using static ClassLibrary.GecolSystem.Models.ConfirmCustomerRspXml;
using System.Xml.Serialization;
using System.Xml;

namespace ClassLibrary.GecolSystem.GecolConvertRsp
{
    public class ConvCreditVendCRsp
    {

        //private static XmlSerializer serializer = new XmlSerializer(typeof(CreditVendRspXml.Envelope));

        //private static DirectDebitUnitRsp directDebitUnitRsp = new DirectDebitUnitRsp();

        //private static BalanceDto balanceDto;


        public static async Task<string> Converte(string SoapRsp) => await Task.Run(() =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(SoapRsp); // Load the XML from the string

                // Create a namespace manager to handle the namespaces in the XML
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns2", "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema");
                nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlElement root = doc.DocumentElement;
                if (root != null)
                {
                    // Select the token node using XPath and namespaces
                    XmlNode tokenNode = root.SelectSingleNode("//ns2:token", nsmgr);
                    if (tokenNode != null && tokenNode["ns2:stsCipher"] != null)
                    {
                        string stsCipher = tokenNode["ns2:stsCipher"].InnerText;
                        //Console.WriteLine("STS Cipher: " + stsCipher);

                        return stsCipher;
                    }
                }
                return null;
            }).ConfigureAwait(false);
        }
    }

