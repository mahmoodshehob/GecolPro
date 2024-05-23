﻿using System.Globalization;
using Newtonsoft.Json;

//Models

//using GecolPro.Main.Models;
//using static GecolPro.Main.Models.MultiRequestUSSD;
using ClassLibrary.Models.Models;
using static ClassLibrary.Models.Models.MultiRequestUSSD;


// Class Library
using ClassLibrary.DCBSystem;
using ClassLibrary.GecolSystem;
using ClassLibrary.Services;
using Menus = GecolPro.Main.BusinessRules.Menus;



namespace GecolPro.Main.BusinessRules
{
    public class UssdProcessV1
    {
        private static Random random = new Random();
        private static Loggers LoggerG = new Loggers();
        private static MsgContent msgContentResult = new MsgContent();
        private static SubProService subProService = new SubProService();
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



        /*Provide USSD and SMS Message Reply :

*/

        private static async Task<MsgContent> MenuReader(SubProService subProService, string Lang)
        {
            var subProServiceResp = await DcbOperation.QryUserBasicBalOp(subProService.MSISDN);

            if (subProServiceResp.Status)
            {

                var BalanceValue = subProServiceResp.Response;

                List<string> outputs = new List<string>();

                outputs.Add(subProService.MeterNumber);
                outputs.Add(subProService.Amount.ToString());
                outputs.Add(subProService.MSISDN);
                outputs.Add(BalanceValue);

                msgContentResult = await Menus.SuccessResponseAsync(outputs, Lang);
                return (msgContentResult);

            }
            else
            {
                msgContentResult = await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode, Lang);

                return (msgContentResult);

            }
        }




        /*Chech if Msisdn Blocked or Not :
         */

        private static async Task<Boolean> BlackListMsisdn(string Msisdn)
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(baseDirectory, "BlackListMsisdn.json");
                var json = File.ReadAllText(jsonFilePath);

                string[]? BlackList = JsonConvert.DeserializeObject<string[]>(json);

                if (!string.IsNullOrEmpty(BlackList.ToString()))
                {
                    if (BlackList.Any(x => x.StartsWith(Msisdn)))
                    {
                        await LoggerG.LogInfoAsync("LynaGclsys|xxx|Msisdn_Blocked|" + subProService.ConversationID + "|Service Connected");
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }




        /*Chech if Gecol System Reachable or Not :
         
        */

        private static async Task<Boolean> CheckServiceExist()
        {
            await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_GecolCheck|" + subProService.ConversationID + "|Check Service Connectivity");

            if ((await GecolOperation.LoginReqOp()).Status)
            {
                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolCheck|" + subProService.ConversationID + "|Service Connected");
                
                return true;
            }

            await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolCheck|" + subProService.ConversationID + "|Service Not Connected");
            
            return false;
        }




        /* API to check if Tirmenated Meter in Gecol Avaiable or not also check if Meter Working :
        //
        // Chech if Meter Exist or not :
        //
        // 1. check first in DataBase.
        //
        // 2. if not exist in DB check in Gecol by API.
        //
        // 3. reply with not exist if not right meter. 
        //
        */

        private static async Task<Boolean> CheckMeterExist(string MeterNumber)
        {
            await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_GecolMeter|" + subProService.ConversationID + "|Check The Meter");

            // Query in DB

            //if ()
            //{ }
            //else if

            if ((await ClassLibrary.GecolSystem.GecolOperation.ConfirmCustomerOp(MeterNumber)).Status)
            {
                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolMeter|" + subProService.ConversationID + "|The Meter Number Connected");

                return true;
            }

            //else 
            //{ }

            await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolMeter|" + subProService.ConversationID + "|The Meter Number Not Exist or has Issue");

            return false;
        }




        /* Charge balance from Billing :
        * 
        * 1. if charging Success reply with true.
        *
        * 2. if charging Failed reply with false.
        *
         */

        private static async Task<TokenOrError> ProcessChargeByDCB(SubProService subProService)
        {
            TokenOrError tokenOrError;

            await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_BillingSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);

            var subProServiceResp = await DcbOperation.DirectDebitUnitOp(subProService.ConversationID, subProService.MSISDN, subProService.Amount);

            await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsq_BillingSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.Status + "|" + subProServiceResp.Response);


            if (subProServiceResp.Status)
            {

                /*here ConncetionString to saveing in DB in success case : 
                 */

                tokenOrError = new TokenOrError()
                {
                    TknOrErr= subProServiceResp.Response,
                    Status = true
                };

                return (tokenOrError);
            }
            else
            {
                /*here ConncetionString to saveing in DB in Failed case :

                                 
                
                 */

                //msgContentResult = await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode, Lang);

                tokenOrError = new TokenOrError()
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = false

                };

                return (tokenOrError);

            }
        }




        /* Order Token From GECOL :
* 
* 1. if token Success reply with true.
*
* 2. if token Failed reply with false.
*
 */

        private static async Task<TokenOrError> ProcessTokenFromGecol(SubProService subProService)
        {
            TokenOrError tokenOrError ;

            await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_GecolVnSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);

            var subProServiceResp = await GecolOperation.CreditVendOp(subProService.MeterNumber, subProService.UniqueNumber, subProService.Amount);

            await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsq_GecolVnSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.Status + "|" + subProServiceResp.Response + "|" + subProService.UniqueNumber);


            /*
            //if (subProServiceResp.Status)
            //{
            //    return (subProServiceResp.Response, subProServiceResp.Status);
            //}
            //else
            //{
            //    return (subProServiceResp.Response, subProServiceResp.Status);
            //}
            */


            if (subProServiceResp.Status)
            {
                /*here ConncetionString to saveing in DB in success case : 
                */

                tokenOrError = new TokenOrError()
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = true

                };

                return (tokenOrError);
            }
            else
            {
                /*here ConncetionString to saveing in DB in Failed case :
                */

                //msgContentResult = await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode);

                tokenOrError = new TokenOrError()
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = false

                };

                return (tokenOrError);

            }

        }




        /* Send SMS API to SMPP Client  :
        */

        public static async void SendGecolMessage(string? sender, string receiver, string message)
        {






            try
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.31.17:8086/api/Messages");

                SmsMessage jsonObject = new SmsMessage()
                {
                    Sender = "2188997772",
                    Receiver = receiver,
                    Message = message
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
                            



        /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
       */

        private static SubProService CreateSubProService(MultiRequest multiRequest , string sessionId)
        {
            string[] Para = multiRequest.USSDRequestString.Split('#');

            //
            //check if the amount value if int or not
            //
            int amount = 0;
            if (int.TryParse(Para[1], out int result))
            {
                amount = result;
            }

            SubProService subProService = new SubProService()
            {
                ConversationID = sessionId,

                MSISDN = multiRequest.MSISDN,

                DateTimeReq = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"),

                MeterNumber = Para[0].ToString(),

                Amount = amount
            };

            return subProService;
        }


        
        
        /* Main Class (Service Start here)
         */

        public static async Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang)
        {
            TokenOrError TokenOrder;

            /* Service start here

             the logic here use two condtions ,
            
             - Generate Sesstion ID : for Subscriber Request
            
             - Check Meter DBs : Check Meter in database if exist  return with true
            
             - Check Meter API : if not in DB check by API if exist add to DB and return with true
            
             - if not exist reply with false
            */



            /* Generate Sesstion ID :
             
            */

            string sessionId = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (DateTime.TryParseExact(multiRequest.TransactionTime, "M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime parsedDate))
            {
                // Format the DateTime object into the desired format
                 sessionId = parsedDate.ToString("yyyyMMddHHmmss");
            }




            /* Logger for start Sesstion :
             */

            await LoggerG.LogInfoAsync($"UssdRequest|Start|Session_Id |{sessionId}");





            /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
            */

            subProService = CreateSubProService(multiRequest, sessionId);




            /* Check BlackList :
 */

            if (await BlackListMsisdn(multiRequest.MSISDN))
            {
                msgContentResult = await Menus.BlockedResponseAsync(Lang);

                return new MultiResponseUSSD()
                {
                    TransactionId = multiRequest.TransactionId,
                    TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                    MSISDN = multiRequest.MSISDN,
                    USSDServiceCode = "0",
                    USSDResponseString = msgContentResult.UssdCont,
                    Action = RespActions.end.ToString(),
                    ResponseCode = 9988
                };
            }
            



            /* API to check if Gecol API Service Avaiable or not also check if our account Working:
             */

            if (await CheckServiceExist())
            {
                //
                //

                /* API to check if Tirmenated Meter in Gecol Avaiable or not also check if Meter Working:
                */

                if (await CheckMeterExist(subProService.MeterNumber))
                {
                    //
                    //

                    /* Create Ussd and SMS Contents:*/

                    try
                    {
                        TokenOrder = await ProcessChargeByDCB(subProService);

                        if (TokenOrder.Status)
                        {

                            TokenOrder = await ProcessTokenFromGecol(subProService);

                            string gecolToken = TokenOrder.TknOrErr;


                            if (TokenOrder.Status)
                            {
                                List<string> outputs = new List<string>();
                                outputs.Add(subProService.MeterNumber);
                                outputs.Add(subProService.Amount.ToString());
                                outputs.Add(gecolToken);
                                outputs.Add(subProService.UniqueNumber);

                                msgContentResult = await Menus.SuccessResponseAsync(outputs, Lang);

                                if (!string.IsNullOrEmpty(msgContentResult.MessageCont))
                                {
                                    SendGecolMessage(null, subProService.MSISDN, msgContentResult.MessageCont);

                                }
                                else
                                {
                                    msgContentResult = await Menus.UnderMaintenance_Billing(TokenOrder.TknOrErr, Lang);
                                }
                            }


                        }
                        else
                        {
                            msgContentResult = await Menus.UnderMaintenance_Billing(TokenOrder.TknOrErr, Lang);
                        }


                    }
                    catch (Exception ex)
                    {
                        msgContentResult.UssdCont = ex.Message;
                        msgContentResult.MessageCont = ex.Message;
                    }
                }
                else
                {
                    msgContentResult = await Menus.UnderMaintenance_Gecol("VD.01010018", Lang);
                }
            }
            else
            {
                msgContentResult= await Menus.UnderMaintenance_Gecol(default, Lang);
            }

            // Generate USSD Response

            await LoggerG.LogInfoAsync($"UssdRequest|Close|Session_Id |{sessionId}");

            var multiResponse = new MultiResponseUSSD()
            {
                TransactionId = multiRequest.TransactionId,
                TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                MSISDN = multiRequest.MSISDN,
                USSDServiceCode = "0",
                USSDResponseString = msgContentResult.UssdCont,
                Action = RespActions.end.ToString(),
            };

            return multiResponse;
        }
    }
}






