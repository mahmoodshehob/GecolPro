using ClassLibrary.DCBSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DCBSystem.DcbCreateXml
{
    public class DirectDebitUnitCreateXml
    {
        private static AuthHeader authHeader = new AuthHeader();

        public static string CreateSoapBody(DirectDebitUnitReqSoap directDebitUnitReqSoap)
        {

            string xmlSoap = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:lib='http://libya.customization.ws.bss.zsmart.ztesoft.com'>
<soapenv:Header>
     <lib:AuthHeader>
        <lib:Username>" + authHeader.Username + @"</lib:Username>
        <lib:Password>" + authHeader.Password + @"</lib:Password>
     </lib:AuthHeader>
  </soapenv:Header>
<soapenv:Body>
     <lib:DirectDebitUnit>
        <lib:DirectDebitUnitReqDto>
           <lib:ConversationID>" + directDebitUnitReqSoap.ConversationID + @"</lib:ConversationID>
           <lib:TransactionID>" + directDebitUnitReqSoap.TransactionID + @"</lib:TransactionID>
           <lib:ServiceName>" + directDebitUnitReqSoap.ServiceName + @"</lib:ServiceName>
           <lib:ProviderName>" + directDebitUnitReqSoap.ProviderName + @"</lib:ProviderName>
           <lib:OriginatingAddress>" + directDebitUnitReqSoap.OriginatingAddress + @"</lib:OriginatingAddress>
           <lib:DestinationAddress>" + directDebitUnitReqSoap.DestinationAddress + @"</lib:DestinationAddress>
           <lib:ChargingAddress>" + directDebitUnitReqSoap.ChargingAddress + @"</lib:ChargingAddress>
           <lib:Amount>" + directDebitUnitReqSoap.Amount + @"</lib:Amount>
        </lib:DirectDebitUnitReqDto>
     </lib:DirectDebitUnit>
  </soapenv:Body>
</soapenv:Envelope>";

            

            return xmlSoap.Replace("'","\"");
        }

    }
}
