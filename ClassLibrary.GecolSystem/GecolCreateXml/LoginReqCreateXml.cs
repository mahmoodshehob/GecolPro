using ClassLibrary.GecolSystem.Models;

namespace ClassLibrary.GecolSystem.GecolCreateXml
{
    public class LoginReqCreateXml
    {
        private static LoginReq loginReq = new LoginReq()
        {
            //Username = ""
        };

        public static string CreateSoapBody()
        {


            string xmlSoap = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
<s:Body xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
<loginReq xmlns='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema'>
<clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='" + loginReq.EANDeviceID + @"' />
<terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='" + loginReq.GenericDeviceID + @"'/>
<msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='20190110150301' uniqueNumber='" + loginReq.UniqueNumber + @"'/>
<authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
<opName>" + loginReq.Username + @"</opName>
<password>" + loginReq.Password + @"</password>
</authCred>
</loginReq>
</s:Body>
</s:Envelope>";

            return xmlSoap.Replace("'", "\"");
            

        }
    }
}