

using static ClassLibrary.Models.UssdModels.MultiRequestUSSD;
using static ClassLibrary.Models.UssdModels.MultiResponseUSSD;

using ClassLibrary.Models.GecolModels;

using ClassLibrary.DCBSystem;

using ClassLibrary.Services;
using System.Globalization;
using ClassLibrary.GecolSystem;
using System;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace GecolPro.Main.ServiceProcess
{
    public class UssdProcessV1
    {
        private static Random random = new Random();

        private static Loggers LoggerG = new Loggers();


        private enum RespActions
        {
            request,
            end
        }

        private enum Respresponse
        {
            True,
            False
        }

        public static async Task<MultiResponse> ServiceProcessing(MultiRequest multiRequest, string Lang)
        {

            await LoggerG.LogInfoAsync("UssdRequest|");

            (string UssdCont, string? MessageCont) Menu;


            string[] Para = multiRequest.USSDRequestString.Split('#');

            //
            //check if the amount value if int or not
            //
            int amount = 0;
            if (int.TryParse(Para[1], out int result))
            {
                amount = result;
            }


            string uniqueNumber = random.Next(1, 999999).ToString("D6");




            SubProService subProService = new SubProService()
            {
                ConversationID = multiRequest.TransactionTime.ToString(),

                MSISDN = multiRequest.MSISDN,

                DateTimeReq = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss").Replace("T",""),

                UniqueNumber = uniqueNumber,

                MeterNumber = Para[0].ToString(),

                Amount = amount

            };


            if (DateTime.TryParseExact(subProService.ConversationID, "M/d/yyyy h:mm:ss tt",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out DateTime parsedDate))
            {
                // Format the DateTime object into the desired format
                subProService.ConversationID = parsedDate.ToString("yyyyMMddHHmmss");
            }



            if (await CheckServiceExist())
            {
                if (await CheckMeterExist(subProService.MeterNumber))
                {
                    // Create Ussd and Message contents
                    try
                    {
                        Menu = await MenuReader(subProService, Lang);
                        //Menu = await ProcessChargeForToken(subProService, Lang);


                        if (!string.IsNullOrEmpty(Menu.MessageCont))
                            SendGecolMessage(null, subProService.MSISDN, Menu.MessageCont);

                    }
                    catch (Exception ex)
                    {
                        Menu.UssdCont = ex.Message;
                        Menu.MessageCont = ex.Message;
                    }
                }
                else
                {
                    Menu.UssdCont = await ServiceProcess.Menus.MeterNotExist(subProService.MeterNumber, Lang);
                }
            }
            else
            {
                Menu.UssdCont = await ServiceProcess.Menus.UnderMaintenance_Gecol(Lang);
            }

            // Generate USSD Response

            var multiResponse = new MultiResponse()
            {
                TransactionId = multiRequest.TransactionId,
                TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                MSISDN = multiRequest.MSISDN,
                USSDServiceCode = "0",
                USSDResponseString = Menu.UssdCont,
                Action = RespActions.end.ToString(),
            };

            return multiResponse;
        }

        private static async Task<(string UssdCont, string? MessageCont)> MenuReader(SubProService subProService, string Lang)
        {
            var subProServiceResp = await DcbOperation.QryUserBasicBalOp(subProService.MSISDN);

            if (subProServiceResp.State)
            {

                var BalanceValue = subProServiceResp.Response;

                List<string> outputs = new List<string>();

                outputs.Add(subProService.MeterNumber);
                outputs.Add(subProService.Amount.ToString());
                outputs.Add(subProService.MSISDN);
                outputs.Add(BalanceValue);

                var Result = await Menus.CheckAsync(outputs, Lang);
                return (Result.UssdCont, Result.MessageCont);

            }
            else
            {

                return (await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode, Lang), null);

            }
        }

        // the logic here use two condtions ,
        //
        // 1. check meter in database if exist  return with true
        //
        // 2. if not in DB check by API if exist add to DB and return with true
        //
        // 3. if not exist reply with false
        //

        private static async Task<Boolean> CheckMeterExist(string MeterNumber)
        {
            // Query in DB


            //if ()
            //{ }
            //else if ()
            //{ }
            //else 
            //{ }
            if ((await ClassLibrary.GecolSystem.GecolOperation.ConfirmCustomerOp(MeterNumber)).Status)
            {
                return true;
            }
            return false;
        }

        private static async Task<Boolean> CheckServiceExist()
        {
            //var Resp = await ClassLibrary.GecolSystem.GecolOperation.LoginReqOp();

            if ((await ClassLibrary.GecolSystem.GecolOperation.LoginReqOp()).Status)
            {
                return true;
            }
            return false;
        }

        public static async void ChargeDcbToGecol()
        {

            string Id = DateTime.Now.ToString("yyyyMMddHHmmss");
            string msisdn = "218921809678";
            int amount = 1;


            var resp = await DcbOperation.DirectDebitUnitOp(Id, msisdn, amount);
        }

        private static async Task<(string UssdCont, string? MessageCont)> ProcessChargeForToken(SubProService subProService, string Lang)
        {
            await LoggerG.LogInfoAsync("ReqToBilling|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);


            var subProServiceResp = await DcbOperation.DirectDebitUnitOp(subProService.ConversationID, subProService.MSISDN, subProService.Amount);

            //(string Response, string StatusCode, Boolean State) subProServiceResp = subProServiceResp = ("OK", "OK", true);


            await LoggerG.LogInfoAsync("RsqToBilling|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.State + "|" + subProServiceResp.Response);



            if (subProServiceResp.State)
            {
                var GecolToken = await ProcessTokenFromGecol(subProService);

                if (GecolToken.Status)
                {

                    List<string> outputs = new List<string>();
                    outputs.Add(subProService.MeterNumber);
                    outputs.Add(subProService.Amount.ToString());
                    outputs.Add(GecolToken.TokenOrError);
                    outputs.Add(subProService.UniqueNumber);

                    var Result = await Menus.CheckAsync(outputs, Lang);

                    return (Result.UssdCont, Result.MessageCont);

                }
                else
                {

                    return (await Menus.UnderMaintenance_Gecol(Lang), null);

                }

            }
            else
            {

                return (await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode, Lang), null);

            }
        }

        public static async void SendGecolMessage(string? sender, string receiver, string message)
        {
            try
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.31.17:8086/api/Messages");

                var jsonObject = new
                {
                    Sender = "2188997772",
                    Receiver = receiver,
                    Massage = message
                };


                var content = new StringContent(JsonConvert.SerializeObject(jsonObject), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var messageResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task<(string TokenOrError,Boolean Status)> ProcessTokenFromGecol(SubProService subProService)
        {
            await LoggerG.LogInfoAsync("ReqToGecol|"+subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount );

            var subProServiceResp = await GecolOperation.CreditVendOp(subProService.MeterNumber, subProService.UniqueNumber, subProService.Amount);

            await LoggerG.LogInfoAsync("RsqToGecol|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.Status + "|" + subProServiceResp.Response);
            
            return (subProServiceResp.Response, subProServiceResp.Status);


            //if (subProServiceResp.Status)
            //{
            //    return (subProServiceResp.Response, subProServiceResp.Status);
            //}
            //else
            //{
            //    return (subProServiceResp.Response, subProServiceResp.Status);
            //}

        }






        //public static async void OrderGecolToken()
        //{
        //    var Soapcli = new ClassLibrary.GecolSystem.SoapServiceClient();
        //    var SoapRsp = await Soapcli.SendSoapRequest(xmlresp, "");
        //    var RespActions = ClassLibrary.GecolSystem.GecolConvertRsp.ConvCreditVendCRsp.Converte(SoapRsp.Responce);
        //    string tkn = RespActions.Result.ToString();



        //}



        //        private static string xmlresp = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
        //<soapenv:Body>
        //<ns2:creditVendReq xmlns:ns2='http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ns2:CreditVendReq'>
        //<clientID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='EANDeviceID' ean='0000000000001'> </clientID>
        //<terminalID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='GenericDeviceID' id='0000000000001'> </terminalID>
        //<msgID xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' dateTime='20230701050523' uniqueNumber='000009'> </msgID>
        //<authCred xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema'>
        //<opName>AG0502</opName>
        //<password>1234567891012</password>
        //</authCred>
        //<resource xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='Electricity'> </resource>
        //<idMethod xmlns='http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema' xsi:type='VendIDMethod'>
        //<meterIdentifier xsi:type='MeterNumber' msno='0268999900262'/>
        //</idMethod>
        //<ns2:purchaseValue xsi:type='ns2:PurchaseValueCurrency'>
        //<ns2:amt value='20' symbol='LYD'/>
        //</ns2:purchaseValue>
        //</ns2:creditVendReq>
        //</soapenv:Body>
        //</soapenv:Envelope>";


    }
}






