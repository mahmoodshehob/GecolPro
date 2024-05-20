using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Serialization;
using GecolPro.Main.Models;
using static GecolPro.Main.Models.MultiRequestUSSD.MultiRequestSerXml;


namespace GecolPro.Main.BusinessRules
{
    internal  class UssdService : IConvertReq,  ICreateXmlResp
    {

        public  async Task<MultiRequestUSSD.MultiRequest> ConverterFaster(string xmlString)
        {

             
            MultiRequestUSSD.MultiRequest multiRequestRE = new MultiRequestUSSD.MultiRequest();
        
            XmlSerializer serializer = new XmlSerializer(typeof(MethodCall));
            
            using (StringReader reader = new StringReader(xmlString))
            {
                await Task.Run(() =>
                {
                    var methodCallReq = (MethodCall)serializer.Deserialize(reader);

                    Struct @struct = methodCallReq.Params.Param.Values.Struct;

                    foreach (var member in @struct.Member)
                    {
                        switch (member.Name)
                        {
                            case "TransactionId":
                                multiRequestRE.TransactionId = member.Value.String;
                                break;

                            case "TransactionTime":
                                if (member.Value.DateTimeIso8601 != null)
                                {
                                    multiRequestRE.TransactionTime = DateTime.ParseExact(member.Value.DateTimeIso8601, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture).ToString(); ;
                                }
                                else
                                {
                                    multiRequestRE.TransactionTime = DateTime.ParseExact(member.Value.String, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture).ToString(); ;
                                }
                                break;

                            case "MSISDN":
                                multiRequestRE.MSISDN = member.Value.String;
                                break;

                            case "USSDServiceCode":
                                multiRequestRE.USSDServiceCode = member.Value.String;
                                break;

                            case "USSDRequestString":
                                multiRequestRE.USSDRequestString = member.Value.String;
                                break;

                            case "response":
                                multiRequestRE.Response = member.Value.String;
                                break;
                        }
                    }
                }).ConfigureAwait(false);
                return multiRequestRE;
            }
        }

        public  string Resp(MultiResponseUSSD multiResponse)
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
<string>" + multiResponse.TransactionId + @"</string>
</value>
</member>
<member>
<name>TransactionTime</name>
<value><dateTime.iso8601>" + DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ssK") + @"</dateTime.iso8601>
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
<string>" + multiResponse.Action + @"</string>
</value>
</member>
</struct>
</value>
</param>
</params>
</methodResponse>";
        }

        public  string Fault_Response(string FaultString, int FaultCode = 4001)
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
 <int>" + FaultCode + @"</int>
 </value>
 </member>
 <member>
 <name>faultString</name>
 <value>
<string>" + FaultString + @"</string>
 </value>
 </member>
 </struct>
</value>
</fault>
</methodResponse>";
        }

    }


    public interface IConvertReq
    {
         Task<MultiRequestUSSD.MultiRequest> ConverterFaster(string xmlString);
    }

    public  interface ICreateXmlResp
    {
        string Resp(MultiResponseUSSD multiResponse);

        string Fault_Response(string FaultString, int FaultCode = 4001);
    }
}
