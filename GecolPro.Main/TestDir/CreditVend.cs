using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GecolPro.Main.Test
{
    public class CreditVend
    {
        public string ToCreditVendCRsp(string xmlSoapResponse)
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlSoapResponse); // Load the XML from the string

            // Create a namespace manager to handle the namespaces in the XML
            XmlNamespaceManager nsmgr = new(doc.NameTable);
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
        }

    }
}
