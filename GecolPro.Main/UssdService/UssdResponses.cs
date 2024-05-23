using ClassLibrary.Models.Models;

namespace GecolPro.Main.UssdService
{
    public class Responses
    {
        public static string Resp(MultiResponseUSSD multiResponse)
        {
            return
 @"<?xml version=""1.0"" encoding=""UTF-8""?>
<methodResponse>
<params>
<param>
<value>
<struct>
<member>
<name>TransactionId</name>
<value>
<string>"+ multiResponse.TransactionId + @"</string>
</value>
</member>
<member>
<name>TransactionTime</name>
<value><dateTime.iso8601>"+DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ssK")+@"</dateTime.iso8601>
</value>
</member>
<member>
<name>USSDResponseString</name>
<value>
<string>" + multiResponse.USSDResponseString + @"</string>
</value>
</member>
<member>
<name>action</name>
<value>
<string>"+multiResponse.Action+@"</string>
</value>
</member>
</struct>
</value>
</param>
</params>
</methodResponse>";
        }

        public static string Fault_Response(MultiResponseUSSD multiResponse)
        {
            return
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<methodResponse>
<fault>
<value>
 <struct>
 <member>
 <name>faultCode</name>
 <value>
 <int>"+ multiResponse .ResponseCode+ @"</int>
 </value>
 </member>
 <member>
 <name>faultString</name>
 <value>
<string>" + multiResponse.USSDResponseString + @"</string>
 </value>
 </member>
 </struct>
</value>
</fault>
</methodResponse>";
        }
    }
}
