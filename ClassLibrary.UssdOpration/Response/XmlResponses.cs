using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.Models.ModelOfUssd;


namespace ClassLibrary.UssdOpration.Response
{
    
    public class XmlResponses
    {
        public string xmlheader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";

        public string Responce(string transId, string mSISDN, string menu, string action)
        {

            MultiResponce multiResponce = new MultiResponce()
            {
                TransactionId = transId,
                TransactionTime = DateTime.UtcNow.ToString("yyyyMMddTHH\\:mm\\:ss"),
                MSISDN = mSISDN,
                USSDResponseString = menu,
                Action = action

            };
          
           string format = xmlheader + @"<methodResponse>
<params>
<param>
<value>
<struct>
<member>
<name>TransactionId</name>
<value>
<string>" + multiResponce.TransactionId + @"</string>
</value>
</member>
<member>
<name>TransactionTime</name>
<value>
<dateTime.iso8601>" + multiResponce.TransactionTime + @"</dateTime.iso8601>
</value>
</member>
<member>
<name>USSDResponseString</name>
<value>
<string>" + multiResponce.USSDResponseString + @"</string>
</value>
</member>
<member>
<name>action</name>
<value>
<string>" + multiResponce.Action + @"</string>
</value>
</member>
</struct>
</value>
</param>
</params>
</methodResponse>";

            return format;
        }


        public string ResponceFuelts(string transId, string ussdReqCode, string menu)
        {

            MultiResponce multiResponce = new MultiResponce()
            {
                TransactionId = transId,
                TransactionTime = DateTime.UtcNow.ToString("yyyyMMddTHH\\:mm\\:ss"),
                USSDResponseString = menu,


            };

            string format = xmlheader + @"<methodResponse>
<fault>
<value>
<struct>
<member>
<name>faultCode</name>
<value>
<int>4001</int>
</value>
</member>
<member>
<name>faultString</name>
<value>
<string>" + multiResponce.USSDResponseString + @"</string>
</value>
</member>
</struct>
</value>
</fault>
</methodResponse>";

            return format;
        }
    }
}