using ClassLibrary.DCBSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary.DCBSystem.DcbCreateXml
{
    public class QryUserBalCreateXml
    {

        private static AuthHeader authHeader = new AuthHeader()
        {
           // Username = "qwe"
        };

        public static string CreateSoapBody(QryUserBasicBalSoap qryUserBasicBalSoap)
        {

            return @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:lib='http://libya.customization.ws.bss.zsmart.ztesoft.com'>
   <soapenv:Header>
      <lib:AuthHeader>
         <lib:Username>" + authHeader.Username + @"</lib:Username>
         <lib:Password>" + authHeader.Password + @"</lib:Password>
      </lib:AuthHeader>
   </soapenv:Header>
   <soapenv:Body>
      <lib:QryUserBasicBal>
         <lib:QryUserBasicBalReqDto>
            <lib:MSISDN>" + qryUserBasicBalSoap.MSISDN + @"</lib:MSISDN>
         </lib:QryUserBasicBalReqDto>
      </lib:QryUserBasicBal>
   </soapenv:Body>
</soapenv:Envelope>
";
        }
    }
}