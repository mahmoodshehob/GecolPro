using System.Xml;
using System.Xml.Serialization;
using ClassLibrary.GecolSystem_Update.Models;

namespace ClassLibrary.GecolSystem_Update
{
    internal class XmlServices : ICreateResponse,ICreateXml
    {
        public async Task<string?> ToCreditVendCRsp(string xmlSoapResponse) => await Task.Run(() =>
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
        }).ConfigureAwait(false);

        public async Task<FaultModel.xmlvendFaultRespFault> ToFaultRsp(string xmlSoapResponse)
        {

            FaultModel.xmlvendFaultRespFault respFault = new();
            XmlDocument doc = new();
            doc.LoadXml(xmlSoapResponse); // Load the XML from the string

            XmlNamespaceManager nsmgr = new(doc.NameTable);
            nsmgr.AddNamespace("ns2", "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema");

            var root = doc.DocumentElement!;
            try
            {
                var list = root.SelectNodes("//ns2:fault", nsmgr)!; // Use XPath to find the right nodes with namespace
                foreach (XmlNode node in list)
                {
                    var faultCode = node["ns2:faultCode"]!.InnerText;

                    var faultDescription = node["ns2:desc"]!.InnerText;

                    respFault = new FaultModel.xmlvendFaultRespFault()
                    {
                        FaultCode = faultCode,
                        Desc = faultDescription
                    };
                    return respFault;
                }
                return respFault;
            }
            catch (Exception ex)
            {
                return new FaultModel.xmlvendFaultRespFault()
                {
                    FaultCode = ex.Message,
                    Desc = ex.Message
                };
            }
        }

        public async Task<LoginRspXml.LoginRsp> ToLoginRsp(string xmlSoapResponse)
        {
            LoginRspXml.LoginRsp loginRsp = new();
            XmlSerializer serializer = new(typeof(LoginRspXml.Envelope));

            using StringReader reader = new(xmlSoapResponse);
            await Task.Run(() =>
            {
                var envelope = (LoginRspXml.Envelope)serializer.Deserialize(reader);

                loginRsp = envelope!.Body.user;

                return loginRsp;

            }).ConfigureAwait(false);

            return loginRsp;
        }

        public string CreateXmlCustomerRequest(string meterNumber)
        {
            var confirmCusReq = new ConfirmCustomerReq
            {
                MeterNumber = meterNumber
            };

            var xmlSoap = $@"
    <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
        <soapenv:Body>
            <ns2:confirmCustomerReq xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:ConfirmCustomerReq'>
            <clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='{confirmCusReq.EANDeviceID}'/>
            <terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='{confirmCusReq.GenericDeviceID}'/>
            <msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='{confirmCusReq.DateTimeReq}' uniqueNumber='{confirmCusReq.UniqueNumber}'/>
            <authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
                <opName>{confirmCusReq.Username}</opName>
                <password>{confirmCusReq.Password}</password>
                <newPassword/>
            </authCred>
            <ns2:idMethod xmlns:ns1='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='ns1:VendIDMethod'>
            <ns1:meterIdentifier xsi:type='ns1:MeterNumber' msno='{confirmCusReq.MeterNumber}'/>
            </ns2:idMethod>
            </ns2:confirmCustomerReq>
        </soapenv:Body>
    </soapenv:Envelope>";

            return xmlSoap;
        }

        public string CreateXmlCreditVendRequest(string meterNumber, string uniqueNumber, int purchaseValue)
        {
            CreditVendReq creditVendReq = new()
            {
                UniqueNumber = uniqueNumber,
                MeterNumber = meterNumber,
                PurchaseValue = purchaseValue
            };

            var xmlSoap = $@"<soapenv:Envelope
    xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
    <soapenv:Body>
        <ns2:creditVendReq
            xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema'
            xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:CreditVendReq'>
            <clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='{creditVendReq.EANDeviceID}' />
            <terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='{creditVendReq.GenericDeviceID}'/>
            <msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='{creditVendReq.DateTimeReq}' uniqueNumber='{creditVendReq.UniqueNumber}'/>
            <authCred
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
                <opName>{creditVendReq.Username}</opName>
                <password>{creditVendReq.Password}</password>
            </authCred>
            <resource
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='Electricity'>
            </resource>
            <idMethod
                xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='VendIDMethod'>
                <meterIdentifier xsi:type='MeterNumber' msno='{creditVendReq.MeterNumber}'></meterIdentifier>
            </idMethod>
            <ns2:purchaseValue xsi:type='ns2:PurchaseValueCurrency'>
                <ns2:amt value='{creditVendReq.PurchaseValue}' symbol='LYD'></ns2:amt>
            </ns2:purchaseValue>
        </ns2:creditVendReq>
    </soapenv:Body>
</soapenv:Envelope>";
            return xmlSoap.Replace("'", "\"");
        }

        public string CreateXmlLoginRequest()
        {
            CommonParameters login = new();

            var xmlSoap = $@"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
<s:Body xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
<loginReq xmlns='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema'>
<clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='{login.EANDeviceID}' />
<terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='{login.GenericDeviceID}'/>
<msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='20190110150301' uniqueNumber='{login.UniqueNumber}'/>
<authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
<opName>{login.Username}</opName>
<password>{login.Password}</password>
</authCred>
</loginReq>
</s:Body>
</s:Envelope>";
            return xmlSoap.Replace("'", "\"");
        }

    }

    public interface ICreateResponse
    {
        Task<string?> ToCreditVendCRsp(string xmlSoapResponse);
        Task<FaultModel.xmlvendFaultRespFault> ToFaultRsp(string xmlSoapResponse);
        Task<LoginRspXml.LoginRsp> ToLoginRsp(string xmlSoapResponse);
    }


    public interface ICreateXml
    {  
        string CreateXmlLoginRequest();
        string CreateXmlCustomerRequest(string meterNumber);
        string CreateXmlCreditVendRequest(string meterNumber, string uniqueNumber, int purchaseValue);
      
    }

}

