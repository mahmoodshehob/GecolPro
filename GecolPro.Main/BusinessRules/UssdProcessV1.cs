using System.Globalization;
using Newtonsoft.Json;

//Models

using ClassLibrary.Models.Models;
using static ClassLibrary.Models.Models.MultiRequestUSSD;


// Class Library
using ClassLibrary.DCBSystem;
using ClassLibrary.GecolSystem;
using ClassLibrary.Services;


namespace GecolPro.Main.BusinessRules
{
    public class UssdProcessV1
    {

        private static Random random = new Random();
        private static Loggers LoggerG = new Loggers();
        private static MsgContent msgContentResult = new MsgContent();
        private static SubProService subProService = new SubProService();
        private static IDcbServices? DcbServices = new DcbServices();
        private static IGecolServices? GecolServices = new GecolServices();
        //private static IDcbServices? DcbServices;
        //private static IGecolServices? GecolServices;


        private static string logPrefix = "LynaGclsys";

        private static string conversationId => subProService.ConversationID;
        private static string transactionID => subProService.TransactionID;


        public UssdProcessV1(IGecolServices _gecolServices , IDcbServices? _dcbServices)
        {
            DcbServices = _dcbServices;
            GecolServices = _gecolServices;
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
                    UssdCont = "Error performing request Unknown Error"
                };
            }
        }




        /*Chech if Msisdn Blocked or Not :
         */

        private static async Task<bool> BlackListMsisdn(string msisdn)
        {
            try
            {

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(baseDirectory, "BlackListMsisdn.json");

                if (!File.Exists(jsonFilePath))
                {
                    await LoggerG.LogInfoAsync($"{logPrefix}|xxx|BlackList_File_Not_Found|{conversationId}|Service Connected");
                    return false;
                }

                string json = await File.ReadAllTextAsync(jsonFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    await LoggerG.LogInfoAsync($"{logPrefix}|xxx|BlackList_Empty_File|{conversationId}|Service Connected");
                    return false;
                }

                string[]? blackList = JsonConvert.DeserializeObject<string[]>(json);

                if (blackList != null && blackList.Any(x => x.StartsWith(msisdn)))
                {
                    await LoggerG.LogInfoAsync($"{logPrefix}|xxx|Msisdn_Blocked|{conversationId}|Service Connected");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return false;
            }

        }




        /*Chech if Gecol System Reachable or Not :
         
        */

        public static async Task<bool> CheckServiceExist()
        {
            try
            {
                await LoggerG.LogInfoAsync($"{logPrefix}==>|Req_GecolCheck|{conversationId}|Check Service Connectivity");

                GecolSystemResponse loginOp = await GecolServices.LoginReqOp();

                if (loginOp.IsSuccessStatusCode)
                {
                    await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{conversationId}|Service Connected");
                    return true;
                }

                await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{conversationId}|Service Not Connected");
                return false;


            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return false;
            }


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

        private static async Task<bool> CheckMeterExist(string MeterNumber)
        {
            try
            {

                string meterNumber = subProService.MeterNumber;

                await LoggerG.LogInfoAsync($"{logPrefix}==>|Req_GecolMeter|{conversationId}|Check The Meter|{meterNumber}");

                // Query in DB
                // Uncomment and implement DB query logic here
                // if (condition)
                // {
                // }
                // else if (otherCondition)
                // {
                // }

                GecolSystemResponse gecolSystem = await GecolServices.ConfirmCustomerOp(MeterNumber);

                if (gecolSystem.IsSuccessStatusCode)
                {
                    await LoggerG.LogInfoAsync($"{logPrefix}<==|Rsp_GecolMeter|{conversationId}|The Meter Connected|{meterNumber}");
                    return true;
                }

                await LoggerG.LogInfoAsync($"{logPrefix}<==|Rsp_GecolMeter|{conversationId}|The Meter Number Not Exist or has Issue|{meterNumber}");
                return false;
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return false;
            }



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

            try
            {

                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;

                await LoggerG.LogInfoAsync($"{logPrefix}|==>|Req_BillingSys|{conversationId}|{msisdn}|{amount}");

                DcbSystemResponse subProServiceResp = await DcbServices.DirectDebitUnitOp(conversationId, msisdn, amount);

                await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_BillingSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccessStatusCode}|{subProServiceResp.Response}");

                if (subProServiceResp.IsSuccessStatusCode)
                {
                    //here ConncetionString to saveing in DB in success case : 
                    return new TokenOrError
                    {
                        TknOrErr = subProServiceResp.Response,
                        Status = true
                    };
                }

                //*here ConncetionString to saveing in DB in Failed case :

                return new TokenOrError
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = false
                };


            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return new TokenOrError
                {
                    TknOrErr = ex.Message,
                    Status = false
                };
            }
        }




        /* Rollback balance with Billing :
        * 
        * 1. if charging Success reply with true.
        *
        * 2. if charging Failed reply with false.
        *
         */

        private static async Task<TokenOrError> ProcessRollBackDCB(SubProService subProService)
        {

            try
            {

                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;

                await LoggerG.LogInfoAsync($"{logPrefix}|==>|Req_RollBackDCB|{conversationId}|{msisdn}|{amount}");

                DcbSystemResponse subProServiceResp = await DcbServices.DebitRollbackOp(conversationId, subProService.TransactionID, msisdn, amount);

                await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_RollBackDCB|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccessStatusCode}|{subProServiceResp.Response}");

                if (subProServiceResp.IsSuccessStatusCode)
                {
                    //here ConncetionString to saveing in DB in success case : 
                    return new TokenOrError
                    {
                        TknOrErr = subProServiceResp.Response,
                        Status = true
                    };
                }

                //*here ConncetionString to saveing in DB in Failed case :

                return new TokenOrError
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = false
                };


            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return new TokenOrError
                {
                    TknOrErr = ex.Message,
                    Status = false
                };
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

            try
            {
                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;
                string uniqeNumber = subProService.UniqueNumber;

                await LoggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolVnSys|{conversationId}|{msisdn}|{amount}");

                var subProServiceResp = await GecolServices.CreditVendOp(subProService.MeterNumber, uniqeNumber, amount);

                await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolVnSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccessStatusCode}|{subProServiceResp.Response}|{uniqeNumber}");


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

                    return new TokenOrError()
                    {
                        TknOrErr = subProServiceResp.Response,
                        Status = true

                    };


                }
                /*here ConncetionString to saveing in DB in Failed case :
                    */

                //msgContentResult = await Menus.UnderMaintenance_Billing(subProServiceResp.StatusCode);

                return new TokenOrError
                {
                    TknOrErr = subProServiceResp.Response,
                    Status = false

                };


            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return new TokenOrError()
                {
                    TknOrErr = ex.Message,
                    Status = false
                };
            }

        }




        /* Send SMS API to SMPP Client  :
        */

        public static async Task SendGecolMessage(string? sender, string receiver, string message)
        {
            try
            {
                HttpClient client = new HttpClient();
                string msisdn = subProService.MSISDN;

                var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.31.17:8086/api/Messages");

                SmsMessage jsonObject = new()
                {
                    Sender = "2188997772",//sender ?? "2188997772",
                    Receiver = receiver,
                    Message = message
                };

                var content = new StringContent(JsonConvert.SerializeObject(jsonObject), null /*System.Text.Encoding.UTF8*/, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);

                await LoggerG.LogInfoAsync($"{logPrefix}|==>|Req_SMSCSystem|Submet|To|{msisdn}");

                response.EnsureSuccessStatusCode();
                var messageResponse = await response.Content.ReadAsStringAsync();

                await LoggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_SMSCSystem|Response|{messageResponse}");
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

            if (Para.Length < 2)
            {
                throw new ArgumentException("The USSD request string does not contain enough parameters.");
            }

            //
            //check if the amount value if int or not
            //
            int amount = 0;
            if (int.TryParse(Para[1], out int result))
            {
                amount = result;
            }

            SubProService subProService = new()
            {
                ConversationID = sessionId,

                MSISDN = multiRequest.MSISDN,

                DateTimeReq = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"),

                MeterNumber = Para[0],

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
            string TransIDms = DateTime.Now.ToString("fff");

            string sessionId = DateTime.Now.ToString($"yyyyMMddHHmmss{TransIDms}");

            if (DateTime.TryParseExact(multiRequest.TransactionTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Format the DateTime object into the desired format
                sessionId = parsedDate.ToString("yyyyMMddHHmmss") + TransIDms;
            }




            /* Logger for start Sesstion :
             */

            await LoggerG.LogInfoAsync($"UssdSystem|==>|Req_{logPrefix}|Start|Session_Id |{sessionId}");





            /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
            */

            subProService = CreateSubProService(multiRequest, sessionId);




            /* Check BlackList :
 */

            if (await BlackListMsisdn(multiRequest.MSISDN))
            {
                msgContentResult = await Menus.BlockedResponseAsync(Lang);

                return new MultiResponseUSSD
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
                        TokenOrder = await ProcessTokenFromGecol(subProService);

                        string gecolToken = TokenOrder.TknOrErr;



                        if (TokenOrder.Status)
                        {
                            
                            //TokenOrder = await ProcessChargeByDCB(subProService);
                            
                            //subProService.TransactionID = TokenOrder.TknOrErr;

                            //
                            TokenOrder.Status = true;

                            if (TokenOrder.Status)
                            {
                                List<string> outputs = new()
                                {
                                    subProService.MeterNumber,
                                    subProService.Amount.ToString(),
                                    gecolToken,
                                    subProService.UniqueNumber
                                };

                                msgContentResult = await Menus.SuccessResponseAsync(outputs, Lang);

                                if (!string.IsNullOrEmpty(msgContentResult.MessageCont))
                                {
                                    SendMessage.SendGecolMessage(null, subProService.MSISDN, msgContentResult.MessageCont, subProService.ConversationID);
                                }
                                else
                                {
                                    msgContentResult = await Menus.UnderMaintenance_Billing(TokenOrder.TknOrErr, Lang);
                                }
                            }
                            else
                            {
                                var tk = ProcessRollBackDCB(subProService);


                                msgContentResult = await Menus.UnderMaintenance_Billing(TokenOrder.TknOrErr, Lang);

                            }
                            

                        }
                        else
                        {
                            msgContentResult = await Menus.UnderMaintenance_Gecol(TokenOrder.TknOrErr, Lang);
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

            await LoggerG.LogInfoAsync($"UssdSystem|<==|Rsp_{logPrefix}|Close|Session_Id |{sessionId}");

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