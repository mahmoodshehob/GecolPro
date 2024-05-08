using ClassLibrary.GecolSystem.Models;
using System.Xml.Linq;


namespace ClassLibrary.GecolSystem.GecolCreateXml
{
    public class ConfirmCustomerReqCreateXml
    {
        private static ConfirmCustomerReq _confirmCusReq = new ConfirmCustomerReq();

        public static string CreateSoapBody(string MeterNumber)
        {
            _confirmCusReq.MeterNumber =  MeterNumber;

            string xmlSoap = @"
    <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
        <soapenv:Body>
            <ns2:confirmCustomerReq xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:ConfirmCustomerReq'>
            <clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='" + _confirmCusReq.EANDeviceID + @"'/>
            <terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='" + _confirmCusReq.GenericDeviceID + @"'/>
            <msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='" + _confirmCusReq.DateTimeReq + @"' uniqueNumber='" + _confirmCusReq.UniqueNumber + @"'/>
            <authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
                <opName>" + _confirmCusReq.Username + @"</opName>
                <password>" + _confirmCusReq.Password + @"</password>
                <newPassword/>
            </authCred>
            <ns2:idMethod xmlns:ns1='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='ns1:VendIDMethod'>
            <ns1:meterIdentifier xsi:type='ns1:MeterNumber' msno='" + _confirmCusReq.MeterNumber + @"'/>
            </ns2:idMethod>
            </ns2:confirmCustomerReq>
        </soapenv:Body>
    </soapenv:Envelope>";

            return xmlSoap;
        }
    }
}