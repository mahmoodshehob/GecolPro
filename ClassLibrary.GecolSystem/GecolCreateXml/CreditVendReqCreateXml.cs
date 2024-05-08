using ClassLibrary.GecolSystem.Models;

namespace ClassLibrary.GecolSystem.GecolCreateXml
{
    public class CreditVendReqCreateXml
    {
        //private static CreditVend _creditVendReq = new CreditVend();

        public static string CreateSoapBody(string MeterNumber, string UniqueNumber , int PurchaseValue)
        {


            CreditVendReq _creditVendReq = new CreditVendReq() 
            { 
                UniqueNumber = UniqueNumber ,
                MeterNumber = MeterNumber,
               PurchaseValue = PurchaseValue
            };

            string xmlSoap = @"<soapenv:Envelope
    xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
    <soapenv:Body>
        <ns2:creditVendReq
            xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema'
            xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:CreditVendReq'>
            <clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='" + _creditVendReq.EANDeviceID + @"' />
            <terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='" + _creditVendReq.GenericDeviceID + @"'/>
            <msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='" + _creditVendReq.DateTimeReq + @"' uniqueNumber='" + _creditVendReq.UniqueNumber + @"'/>
            <authCred
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
                <opName>" + _creditVendReq.Username + @"</opName>
                <password>" + _creditVendReq.Password + @"</password>
            </authCred>
            <resource
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='Electricity'>
            </resource>
            <idMethod
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='VendIDMethod'>
                <meterIdentifier xsi:type='MeterNumber' msno='" + _creditVendReq.MeterNumber + @"'></meterIdentifier>
            </idMethod>
            <ns2:purchaseValue xsi:type='ns2:PurchaseValueCurrency'>
                <ns2:amt value='" + _creditVendReq.PurchaseValue + @"' symbol='LYD'></ns2:amt>
            </ns2:purchaseValue>
        </ns2:creditVendReq>
    </soapenv:Body>
</soapenv:Envelope>";
            return xmlSoap.Replace("'","\"");
        }
    }
}