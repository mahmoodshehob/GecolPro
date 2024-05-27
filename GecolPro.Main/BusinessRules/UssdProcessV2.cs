using System.Globalization;
using Newtonsoft.Json;

//Models

using ClassLibrary.Models.Models;
using static ClassLibrary.Models.Models.MultiRequestUSSD;


// Class Library
using ClassLibrary.DCBSystem_Update;
using ClassLibrary.GecolSystem_Update;
using ClassLibrary.Services;


namespace GecolPro.Main.BusinessRules
{
    public class UssdProcessV2
    {
        private static Random random = new Random();
        private static Loggers LoggerG = new Loggers();
        private static MsgContent msgContentResult = new MsgContent();
        private static SubProService subProService = new SubProService();
        private static IDcbServices? DcbServices = new DcbServices();
        private static IGecolServices? GecolServices= new GecolServices();

        public UssdProcessV2()
        {

        }



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
            try
            {
                DcbSystemResponse subProServiceResp = await DcbServices.QryUserBasicBalOp(subProService.MSISDN);

                if (subProServiceResp.IsSuccessStatusCode)
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
            catch (Exception ex) 
            {
                await ExceptionLogs(ex);

                return new MsgContent()
                {
                    UssdCont ="Error performing request Unknown Error"
                };
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
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
            }
            return false;
        }




        /*Chech if Gecol System Reachable or Not :
         
        */

        private static async Task<Boolean> CheckServiceExist()
        {
            try
            {
                await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_GecolCheck|" + subProService.ConversationID + "|Check Service Connectivity");

                GecolSystemResponse loginOp = await GecolServices.LoginReqOp();

                if (loginOp.IsSuccessStatusCode)
                {
                    await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolCheck|" + subProService.ConversationID + "|Service Connected");

                    return true;
                }

                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolCheck|" + subProService.ConversationID + "|Service Not Connected");

            }
            catch (Exception ex)
            {
              await  ExceptionLogs(ex);
            }

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
            try
            {
                await LoggerG.LogInfoAsync($"LynaGclsys|==>|Req_GecolMeter|{subProService.ConversationID}|Check The Meter  |{subProService.MeterNumber}");

                // Query in DB

                //if ()
                //{ }
                //else if

                GecolSystemResponse gecolSystem = await GecolServices.ConfirmCustomerOp(MeterNumber);

                if (gecolSystem.IsSuccessStatusCode)
                {
                    await LoggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_GecolMeter|{subProService.ConversationID}|The Meter Connected|{subProService.MeterNumber}");

                    return true;
                }

                //else 
                //{ }

                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsp_GecolMeter|" + subProService.ConversationID + "|The Meter Number Not Exist or has Issue");
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
            }


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
            
            try
            {


                await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_BillingSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);

                DcbSystemResponse subProServiceResp = await DcbServices.DirectDebitUnitOp(subProService.ConversationID, subProService.MSISDN, subProService.Amount);

                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsq_BillingSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.IsSuccessStatusCode + "|" + subProServiceResp.Response);


                if (subProServiceResp.IsSuccessStatusCode)
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

                    //msgContentResult = await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode, Lang);

                    tokenOrError = new TokenOrError()
                    {
                        TknOrErr = subProServiceResp.Response,
                        Status = false

                    };

                    return (tokenOrError);

                }

            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return (new TokenOrError() 
                {
                    TknOrErr= ex.Message,
                    Status = false
                });
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
            TokenOrError tokenOrError;
            int maxRetries = 3;
            int attempt = 0;


            try
            {
                while (attempt < maxRetries)
                {
                    attempt++;

                    await LoggerG.LogInfoAsync("LynaGclsys|==>|Req_GecolVnSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);

                var subProServiceResp = await GecolServices.CreditVendOp(subProService.MeterNumber, subProService.UniqueNumber, subProService.Amount);

                await LoggerG.LogInfoAsync("LynaGclsys|<==|Rsq_GecolVnSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount + "|" + subProServiceResp.IsSuccessStatusCode + "|" + subProServiceResp.Response + "|" + subProService.UniqueNumber);


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


                if (subProServiceResp.IsSuccessStatusCode)
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

                        await LoggerG.LogInfoAsync("LynaGclsys|xxx|Failed+attempt|" + attempt + " for Req_GecolVnSys|" + subProService.ConversationID + "|" + subProService.MSISDN + "|" + subProService.Amount);

                    }
                }
                return tokenOrError = new TokenOrError()
                {
                    TknOrErr = "atmpsfailed",
                    Status = false
                };
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return (new TokenOrError()
                {
                    TknOrErr = ex.Message,
                    Status = false
                });
            }

        }




        /* Send SMS API to SMPP Client  :
        */

        public static async void SendGecolMessage(string? sender, string receiver, string message)
        {
            try
            {

                if (!string.IsNullOrEmpty(message))
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

                    await LoggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Submet|To|{subProService.MSISDN}");

                    response.EnsureSuccessStatusCode();
                    var messageResponse = await response.Content.ReadAsStringAsync();

                    await LoggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);


            }
        }




        /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
       */

        private static SubProService CreateSubProService(MultiRequest multiRequest, string sessionId)
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




        /*Chech Exception Logs :
        */

        private static async Task ExceptionLogs(Exception ex)
        {
            await LoggerG.LogDebugAsync(
                  $"excp" +
                  $"\n{subProService.ConversationID}|{ex.Message}" +
                  $"\n{subProService.ConversationID}|{ex.InnerException}" +
                  $"\n{subProService.ConversationID}|{ex.StackTrace.ToString()}"
                  );
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

            if (DateTime.TryParseExact(multiRequest.TransactionTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Format the DateTime object into the desired format
                sessionId = parsedDate.ToString("yyyyMMddHHmmss");
            }




            /* Logger for start Sesstion :
             */

            await LoggerG.LogInfoAsync($"UssdSystem|==>|Req_LynaGclsys|Start|Session_Id |{sessionId}");





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
                                    SendGecolMessage(null, subProService.MSISDN, msgContentResult.MessageCont);

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
                msgContentResult = await Menus.UnderMaintenance_Gecol(default, Lang);
            }

            // Generate USSD Response

            await LoggerG.LogInfoAsync($"UssdSystem|<==|Rsp_LynaGclsys|Close|Session_Id |{sessionId}");

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






