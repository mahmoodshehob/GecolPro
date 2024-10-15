using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GecolPro.DCBSystem.Models;



namespace GecolPro.DCBSystem
{
    public class XmlServices:  XmlServices.ICreateResponse, XmlServices.ICreateXml
    {
        private static string OrganizeXmlString(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    " // Use four spaces for indentation
            };

            using (XmlWriter writer = XmlWriter.Create(stringBuilder, settings))
            {
                xmlDoc.WriteTo(writer);
            }

            string afterOrganizeXml = stringBuilder.ToString();

            return stringBuilder.ToString();
        }

        public async Task<QryUserBasicBalRsp> ToQryUserBasicRsp(string xmlSoapResponse)
        {
            XmlSerializer serializer = new(typeof(QryUserBasicBalRspXml.Envelope));
            QryUserBasicBalRsp qryUserBasicBalRsp = new();

            using StringReader reader = new(xmlSoapResponse);
            await Task.Run(() =>
            {
                var RespQryUserBasicBal = (QryUserBasicBalRspXml.Envelope)serializer.Deserialize(reader);


                qryUserBasicBalRsp = new QryUserBasicBalRsp()
                {
                    BalanceDto = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.BalanceDtoList.BalanceDto,
                    MSISDN = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.MSISDN
                };
            }).ConfigureAwait(false);


            return qryUserBasicBalRsp;
        }

        public async Task<DirectDebitUnitRsp> ToDirectDebitUnitCRsp(string xmlSoapResponse)
        {
            XmlSerializer serializer = new(typeof(DirectDebitUnitRspXml.Envelope));
            var directDebitUnitRsp = new DirectDebitUnitRsp();
            using StringReader reader = new(xmlSoapResponse);
            await Task.Run(() =>
            {
                var respDirectDebitUnit = (DirectDebitUnitRspXml.Envelope)serializer.Deserialize(reader);

                directDebitUnitRsp = new DirectDebitUnitRsp()
                {
                    ConversationID = respDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.ConversationID,
                    TransactionID = respDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.TransactionID,
                    Amount = respDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.Amount,
                };

                return respDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp;


            }).ConfigureAwait(false);
            return directDebitUnitRsp;
        }

        public async Task<DebitRollBackRsp> ToDebitRollbackRsp(string xmlSoapResponse)
        {
            XmlSerializer serializer = new(typeof(DebitRollbackRspXml.Envelope));
            var debitRollBackRsp = new DebitRollBackRsp();
            using StringReader reader = new(xmlSoapResponse);
            await Task.Run(() =>
            {
                var respDebitRollback = (DebitRollbackRspXml.Envelope)serializer.Deserialize(reader);

                debitRollBackRsp = new DebitRollBackRsp()
                {
                    ConversationID = respDebitRollback.Body.DebitRollbackResponse.DebitRollbackResp.ConversationID,
                    TransactionID = respDebitRollback.Body.DebitRollbackResponse.DebitRollbackResp.TransactionID,
                    Amount = respDebitRollback.Body.DebitRollbackResponse.DebitRollbackResp.Amount,
                };

                return respDebitRollback.Body.DebitRollbackResponse.DebitRollbackResp;


            }).ConfigureAwait(false);

            return debitRollBackRsp;
        }

        public async Task<FaultModel> ToFaultRsp(string xmlSoapResponse)
        {            
            var reqElement = new FaultModel();

            XNamespace soapen = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ns1 = "http://xml.apache.org/axis/";
            var doc = XDocument.Parse(xmlSoapResponse);

            var responceObject = doc.Element(soapen + "Envelope")?.Element(soapen + "Body")?.Descendants(soapen + "Fault");

            reqElement.FaultCode = responceObject.Descendants("faultcode").FirstOrDefault().Value;
            reqElement.FaultString = responceObject.Descendants("faultstring").FirstOrDefault().Value;
            reqElement.Detail = responceObject.Elements("detail").Descendants(ns1 + "hostname").FirstOrDefault().Value;

            return reqElement;
        }




        public string CreateXmlQryUserBal(QryUserBasicBalSoap qryUserBasicBalSoap)
        {
            var authHeader = new AuthHeader();

            var xmlSoap = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:lib='http://libya.customization.ws.bss.zsmart.ztesoft.com'>
   <soapenv:Header>
      <lib:AuthHeader>
         <lib:Username>{authHeader.Username}</lib:Username>
         <lib:Password>{authHeader.Password}</lib:Password>
      </lib:AuthHeader>
   </soapenv:Header>
   <soapenv:Body>
      <lib:QryUserBasicBal>
         <lib:QryUserBasicBalReqDto>
            <lib:MSISDN>{qryUserBasicBalSoap.MSISDN}</lib:MSISDN>
         </lib:QryUserBasicBalReqDto>
      </lib:QryUserBasicBal>
   </soapenv:Body>
</soapenv:Envelope>
";

            return OrganizeXmlString(xmlSoap.Replace("'", "\""));

        }

        public string CreateXmlDirectDebitUnit(DirectDebitUnitReqSoap directDebitUnitReqSoap)
        {
            var authHeader = new AuthHeader();

            var xmlSoap = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:lib='http://libya.customization.ws.bss.zsmart.ztesoft.com'>
<soapenv:Header>
     <lib:AuthHeader>
        <lib:Username>{authHeader.Username}</lib:Username>
        <lib:Password>{authHeader.Password}</lib:Password>
     </lib:AuthHeader>
  </soapenv:Header>
<soapenv:Body>
     <lib:DirectDebitUnit>
        <lib:DirectDebitUnitReqDto>
           <lib:ConversationID>{directDebitUnitReqSoap.ConversationID}</lib:ConversationID>
           <lib:TransactionID>{directDebitUnitReqSoap.TransactionID} </lib:TransactionID>
           <lib:ServiceName>{directDebitUnitReqSoap.ServiceName}</lib:ServiceName>
           <lib:ProviderName>{directDebitUnitReqSoap.ProviderName}</lib:ProviderName>
           <lib:OriginatingAddress>{directDebitUnitReqSoap.OriginatingAddress}</lib:OriginatingAddress>
           <lib:DestinationAddress>{directDebitUnitReqSoap.DestinationAddress}</lib:DestinationAddress>
           <lib:ChargingAddress>{directDebitUnitReqSoap.ChargingAddress}</lib:ChargingAddress>
           <lib:Amount>{directDebitUnitReqSoap.Amount}</lib:Amount>
        </lib:DirectDebitUnitReqDto>
     </lib:DirectDebitUnit>
  </soapenv:Body>
</soapenv:Envelope>";



            return OrganizeXmlString(xmlSoap.Replace("'", "\""));
        }

        public string CreateXmlDebitRollback(DebitRollbackReqSoap debitRollbackReqSoap)
        {
            var authHeader = new AuthHeader();

            var xmlSoap = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:lib='http://libya.customization.ws.bss.zsmart.ztesoft.com'>
<soapenv:Header>
     <lib:AuthHeader>
        <lib:Username>{authHeader.Username}</lib:Username>
        <lib:Password>{authHeader.Password}</lib:Password>
     </lib:AuthHeader>
  </soapenv:Header>
<soapenv:Body>
     <lib:DebitRollbackUnit>
        <lib:DebitRollbackUnitReqDto>
           <lib:ConversationID>{debitRollbackReqSoap.ConversationID}</lib:ConversationID>
           <lib:TransactionID>{debitRollbackReqSoap.TransactionID}</lib:TransactionID>
           <lib:ServiceName>{debitRollbackReqSoap.ServiceName}</lib:ServiceName>
           <lib:ProviderName>{debitRollbackReqSoap.ProviderName}</lib:ProviderName>
           <lib:OriginatingAddress>{debitRollbackReqSoap.OriginatingAddress}</lib:OriginatingAddress>
           <lib:DestinationAddress>{debitRollbackReqSoap.DestinationAddress}</lib:DestinationAddress>
           <lib:ChargingAddress>{debitRollbackReqSoap.ChargingAddress}</lib:ChargingAddress>
           <lib:Amount>{debitRollbackReqSoap.Amount}</lib:Amount>
        </lib:DebitRollbackUnitReqDto>
     </lib:DebitRollbackUnit>
  </soapenv:Body>
</soapenv:Envelope>";



            return xmlSoap.Replace("'", "\"");
        }

   


        public interface ICreateResponse
        {
            Task<DirectDebitUnitRsp> ToDirectDebitUnitCRsp(string xmlSoapResponse);
            
            Task<DebitRollBackRsp> ToDebitRollbackRsp(string xmlSoapResponse);
            
            Task<FaultModel> ToFaultRsp(string xmlSoapResponse);
            
            Task<QryUserBasicBalRsp> ToQryUserBasicRsp(string xmlSoapResponse);
        }


        public interface ICreateXml
        {
            string CreateXmlQryUserBal(QryUserBasicBalSoap qryUserBasicBalSoap);

            string CreateXmlDirectDebitUnit(DirectDebitUnitReqSoap directDebitUnitReqSoap);

            string CreateXmlDebitRollback(DebitRollbackReqSoap debitRollbackReqSoap);
        }
    }
}
