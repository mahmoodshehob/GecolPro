using System.Text;
using Microsoft.Extensions.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GecolPro.Models.Gecol;
using static GecolPro.Models.Gecol.CreditVendRespBody;


namespace GecolPro.GecolSystem
{
    public class XmlServices : IGecolCreateResponse, IGecolCreateXml
    {
        // Define namespaces to simplify queries

        private readonly XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";

        private readonly XNamespace ns2 = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema";

        private readonly XNamespace ns3 = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema";

        private readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        //


        // AuthCred instance to hold configuration values
        private readonly AuthCred _authCred;

        // Constructor accepting IOptions<AuthCred>
        public XmlServices(IConfiguration config)
        {


            //_authCred = authCredOptions.Value ?? throw new ArgumentNullException(nameof(authCredOptions));
            _authCred = new AuthCred();
            config.GetSection("AuthHeaderOfGecol").Bind(_authCred);

        }

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




        // Request

        public string CreateXmlLoginRequest()
        {

            CommonParameters login = new()
            {
                Username = _authCred.Username, 
                Password = _authCred.Password,
                Url = _authCred.Url,
                EANDeviceID = _authCred.EANDeviceID,
                GenericDeviceID = _authCred.GenericDeviceID,

            };




            var xmlSoap = $@"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
<s:Body xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
<loginReq xmlns='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema'>
<clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='{login.EANDeviceID}' />
<terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='{login.GenericDeviceID}'/>
<msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='{login.DateTimeReq}' uniqueNumber='{login.UniqueNumber}'/>
<authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
<opName>{login.Username}</opName>
<password>{login.Password}</password>
</authCred>
</loginReq>
</s:Body>
</s:Envelope>";


            return OrganizeXmlString(xmlSoap.Replace("'", "\""));
        }

        public string CreateXmlCustomerRequest(string meterNumber)
        {
            var confirmCusReq = new ConfirmCustomerReq
            {
                Username= _authCred.Username, 
                Password= _authCred.Password,
                EANDeviceID= _authCred.EANDeviceID,
                GenericDeviceID= _authCred.GenericDeviceID,
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

            return OrganizeXmlString(xmlSoap.Replace("'", "\""));
        }

        public string CreateXmlCreditVendRequest(string meterNumber, string uniqueNumber, int purchaseValue)
        {
            CreditVendReq creditVendReq = new()
            {
                Username = _authCred.Username,
                Password = _authCred.Password,
                EANDeviceID = _authCred.EANDeviceID,
                GenericDeviceID = _authCred.GenericDeviceID,

                UniqueNumber = uniqueNumber,
                MeterNumber = meterNumber,
                PurchaseValue = purchaseValue
            };

            var xmlSoap = $@"
            <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
            <soapenv:Body>
            <ns2:creditVendReq xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:CreditVendReq'>
            <clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='{creditVendReq.EANDeviceID}'>
            </clientID>
            <terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='{creditVendReq.GenericDeviceID}'>
            </terminalID>
            <msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='{creditVendReq.DateTimeReq}' uniqueNumber='{creditVendReq.UniqueNumber}'>
            </msgID>
            <authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
            <opName>{creditVendReq.Username}</opName>
            <password>{creditVendReq.Password}</password>
            </authCred>
            <resource xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='Electricity'>
            </resource>
            <idMethod xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='VendIDMethod'>
            <meterIdentifier xsi:type='MeterNumber' msno='{creditVendReq.MeterNumber}'></meterIdentifier>
            </idMethod>
            <ns2:purchaseValue xsi:type='ns2:PurchaseValueCurrency'>
            <ns2:amt value='{creditVendReq.PurchaseValue}' symbol='LYD'></ns2:amt>
            </ns2:purchaseValue>
            </ns2:creditVendReq>
            </soapenv:Body>
            </soapenv:Envelope>";






            string checkbody = OrganizeXmlString(xmlSoap.Replace("'", "\""));

            return xmlSoap.Replace("'", "\"");
        }

        // Responce

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

        public async Task<ConfirmCustomerRespBody> ToCreateXmlCustomerCRsp(string xmlSoapResponse) => await Task.Run(() =>
        {
            // Parse the XML using XDocument
            XDocument xdoc = XDocument.Parse(xmlSoapResponse);

            // Query the XML document

            var confirmCustResult = xdoc.Descendants(ns3 + "meterDetail").FirstOrDefault();

            // Meter Detail

            string meterDetail_krn = confirmCustResult.Attribute("krn")?.Value;
            string meterDetail_msno = confirmCustResult.Attribute("msno")?.Value;
            string meterDetail_sgc = confirmCustResult.Attribute("sgc")?.Value;
            string meterDetail_ti = confirmCustResult.Attribute("ti")?.Value;

            // Meter Type

            string meterType_At = confirmCustResult.Element(ns2 + "meterType")?.Attribute("at")?.Value;
            string meterType_Tt = confirmCustResult.Element(ns2 + "meterType")?.Attribute("tt")?.Value;

            ConfirmCustomerRespBody confirmCustomerResp = new()
            {
                CustomerNumber = meterDetail_msno,
                AT = meterType_At,
                TT = meterType_Tt,
            };



            return confirmCustomerResp;

        }).ConfigureAwait(false);

        public async Task<CreditVendRespBody.CreditVendResp> ToCreditVendCRsp(string xmlSoapResponse) => await Task.Run(() =>
        {
            // Parse the XML using XDocument
            XDocument xdoc = XDocument.Parse(xmlSoapResponse);

            CreditVendRespBody.CreditVendResp creditVendResp = new CreditVendRespBody.CreditVendResp();

            // Query the XML document

            creditVendResp.ClientID = xdoc.Descendants(ns2 + "clientID").FirstOrDefault()?.Attribute("ean")?.Value;

            creditVendResp.ServerID = xdoc.Descendants(ns2 + "serverID").FirstOrDefault()?.Attribute("ean")?.Value;

            creditVendResp.TerminalID = xdoc.Descendants(ns2 + "terminalID").FirstOrDefault()?.Attribute("id")?.Value;

            creditVendResp.ReqMsgID = new ReqMsgID()
            {
                Datetime = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("dateTime")?.Value,

                UniqueNumber = xdoc.Descendants(ns2 + "reqMsgID").FirstOrDefault()?.Attribute("uniqueNumber")?.Value
            };

            creditVendResp.RespDateTime = xdoc.Descendants(ns2 + "respDateTime").FirstOrDefault()?.Value;

            creditVendResp.DispHeader = xdoc.Descendants(ns2 + "dispHeader").FirstOrDefault()?.Value.Split('|')[0];

            creditVendResp.ClientStatus = xdoc.Descendants(ns2 + "availCredit").FirstOrDefault()?.Attribute("value")?.Value;

            creditVendResp.CustVendAccNo = xdoc.Descendants(ns2 + "custVendDetail").FirstOrDefault()?.Attribute("accNo")?.Value;


            creditVendResp.CreditVendReceipt = xdoc.Descendants(ns3 + "creditVendReceipt").FirstOrDefault()?.Attribute("receiptNo")?.Value;

            creditVendResp.TenderAmount = xdoc.Descendants(ns3 + "tenderAmt").Attributes("value").First().Value;

            var transactions = xdoc.Descendants(ns3 + "tx").ToList();

            //////////////////////////
            ///
            /// Transactions / CreditVendTx
            ///
            //////////////////////////
            XElement trans_CreditVendTx = transactions[0];

            XElement? trans_RecoveryTx = null;

            List<XElement> trans_ServiceChrgTx = new List<XElement>();


            for (int i = 1; i < transactions.Count; i++)
            {
                string _attrib = transactions[i].Attribute(xsi + "type")?.Value;

                if (_attrib == "ns3:DebtRecoveryTx")
                {

                    trans_RecoveryTx = transactions[i];
                }
                if (_attrib == "ns3:ServiceChrgTx")
                {
                    trans_ServiceChrgTx.Add(transactions[i]);
                }
            }

            //////////////////////////
            ///
            ///  Transactions / CreditVendTx / Credit Token Issue
            ///  
            ///  Transactions / CreditVendTx / KC Token Issue
            ///
            //////////////////////////


            string KcTokenDesc = null;
            string KcTokenIssueSet1 = null;
            string KcTokenIssueSet2 = null;



            if (trans_CreditVendTx.Element(ns3 + "kcTokenIssue") != null)
            {
                KcTokenDesc = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "desc").Value;
                KcTokenIssueSet1 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set1stMeterKey").Element(ns2 + "stsCipher").Value;
                KcTokenIssueSet2 = trans_CreditVendTx.Element(ns3 + "kcTokenIssue").Element(ns2 + "token").Element(ns2 + "set2ndMeterKey").Element(ns2 + "stsCipher").Value;
            }

            creditVendResp.CreditVendTx = new TransactionsCreditVendTx()
            {
                Amout = trans_CreditVendTx.Element(ns3 + "amt").Attribute("value")?.Value,
                Symbol = trans_CreditVendTx.Element(ns3 + "amt").Attribute("symbol")?.Value,
                Desc_CToken = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "desc").Value,
                STS1Token = trans_CreditVendTx.Element(ns3 + "creditTokenIssue").Element(ns2 + "token").Element(ns2 + "stsCipher").Value,
                Desc_KcToken = KcTokenDesc,
                Set1stMeterKey = KcTokenIssueSet1,
                Set2ndMeterKey = KcTokenIssueSet2,
                Tariff = trans_CreditVendTx.Element(ns3 + "tariff").Element(ns2 + "name").Value,
            };




            //////////////////////////
            ///
            ///  Transactions / RecoveryTx
            ///
            //////////////////////////


            if (trans_RecoveryTx != null)
            {
                creditVendResp.RecoveryTx = new TransactionsDebtRecoveryTx()
                {
                    AccDesc = trans_RecoveryTx.Element(ns3 + "accDesc").Value,
                    Amout = trans_RecoveryTx.Element(ns3 + "amt").Attribute("value")?.Value,
                    Tariff = trans_RecoveryTx.Element(ns3 + "tariff").Value,
                    Balance = trans_RecoveryTx.Element(ns3 + "balance").Attribute("value")?.Value

                };
            }



            //////////////////////////
            ///
            ///  Transactions / ServiceChrgTx
            ///
            //////////////////////////


            if (trans_ServiceChrgTx == null || trans_ServiceChrgTx.Count == 0)
            {
                creditVendResp.ServiceChrgTx = null;
            }
            else if (trans_ServiceChrgTx != null)
            {
                creditVendResp.ServiceChrgTx = new List<TransactionsServiceChrgTx>();

                foreach (var trans_ServiceChrgTxp in trans_ServiceChrgTx)
                {

                    TransactionsServiceChrgTx serviceChrgTx = new TransactionsServiceChrgTx()
                    {
                        AccDesc = trans_ServiceChrgTxp.Element(ns3 + "accDesc").Value,
                        Amout = trans_ServiceChrgTxp.Element(ns3 + "amt").Attribute("value")?.Value,
                        Tariff = trans_ServiceChrgTxp.Element(ns3 + "tariff").Value,

                    };


                    if (serviceChrgTx.AccDesc.Contains("everymonth * "))
                    {
                        var getNumOfMonth = serviceChrgTx.AccDesc.Split('*');

                        int NumOfMonthExpLastMonth = int.Parse(getNumOfMonth[1].Trim());

                        serviceChrgTx.Amout = (int.Parse(serviceChrgTx.Amout) * NumOfMonthExpLastMonth).ToString();

                        serviceChrgTx.AccDesc = "Monthly Tax for old (" + NumOfMonthExpLastMonth + ") months";
                    }

                    creditVendResp.ServiceChrgTx.Add(serviceChrgTx);
                }
            }


            return creditVendResp;

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
    }

    public interface IGecolCreateResponse
    {
        Task<LoginRspXml.LoginRsp> ToLoginRsp(string xmlSoapResponse);
        Task<ConfirmCustomerRespBody> ToCreateXmlCustomerCRsp(string xmlSoapResponse);
        Task<CreditVendRespBody.CreditVendResp?> ToCreditVendCRsp(string xmlSoapResponse);
        Task<FaultModel.xmlvendFaultRespFault> ToFaultRsp(string xmlSoapResponse);
    }

    public interface IGecolCreateXml
    {  
        string CreateXmlLoginRequest();
        string CreateXmlCustomerRequest(string meterNumber);
        string CreateXmlCreditVendRequest(string meterNumber, string uniqueNumber, int purchaseValue); 
    }
}

