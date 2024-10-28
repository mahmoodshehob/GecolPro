using System.Globalization;
using Newtonsoft.Json;

//Models

using GecolPro.Models.Models;
using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;


using static GecolPro.Models.Models.MultiRequestUSSD;


// Class Library
using GecolPro.DCBSystem;
using GecolPro.GecolSystem;
using GecolPro.Services.IServices;
using GecolPro.WebApi.Interfaces;






namespace GecolPro.WebApi.BusinessRules
{
    public class UssdProcess : IUssdProcess
    {

        private readonly AuthHeader _authHeader;
        private readonly Random random = new Random();

        private ILoggers _loggerG ;
        private IDcbServices? _dcbServices;
        private IGecolServices? _gecolServices;
        private IMenusX? _menus;
        private ISendMessage? _sendMessage;


        private MsgContent msgContentResult = new MsgContent();
        private SubProService subProService = new SubProService();



        //
        private const string logPrefix = "LynaGclsys";



        //
        private string conversationId => subProService.ConversationID;
        private string transactionID => subProService.TransactionID;


        public UssdProcess(
            IGecolServices  gecolServices , 
            IDcbServices?   dcbServices,
            ILoggers        loggerG,
            IMenusX          menus,
            ISendMessage    sendMessage,
            IConfiguration _config)
        {
            _dcbServices = dcbServices;
            _gecolServices = gecolServices;
            _loggerG = loggerG;
            _menus = menus;
            _sendMessage = sendMessage;
            _authHeader = new AuthHeader()
            {
                Username = _config.GetValue<string>("AuthHeaderOfDCB:username"),
                Password = _config.GetValue<string>("AuthHeaderOfDCB:password"),
                Url      = _config.GetValue<string>("AuthHeaderOfDCB:url")
            };
        }

        private class RespActions
        {
            public const string request = "request";
            public const string end = "end";

        }


        private class Respresponse
        {
            public const string True = "True";
            public const string False = "False";  
        }

        /*Chech if Msisdn Blocked or Not :
         */

        private async Task<bool> BlackListMsisdn(string msisdn)
        {
            try
            {

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(baseDirectory, "BlackListMsisdn.json");

                if (!File.Exists(jsonFilePath))
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|xxx|BlackList_File_Not_Found|{conversationId}|Service Connected");
                    return false;
                }

                string json = await File.ReadAllTextAsync(jsonFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|xxx|BlackList_Empty_File|{conversationId}|Service Connected");
                    return false;
                }

                string[]? blackList = JsonConvert.DeserializeObject<string[]>(json);

                if (blackList != null && blackList.Any(x => x.StartsWith(msisdn)))
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|xxx|Msisdn_Blocked|{conversationId}|Service Connected");
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

        public async Task<bool> CheckServiceExist()
        {
            try
            {
                await _loggerG.LogInfoAsync($"{logPrefix}==>|Req_GecolCheck|{conversationId}|Check Service Connectivity");

                var loginOp = await _gecolServices.LoginReqOpx();

                if (loginOp.IsSuccess)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{conversationId}|Service Connected");
                    return true;
                }

                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{conversationId}|Service Not Connected");
                return false;


            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
                return false;
            }


        }





        //        /*Chech if Gecol System Reachable or Not :

        //*/

        //        public async Task<bool> CheckDcbExist()
        //        {
        //            try
        //            {
        //                await _loggerG.LogInfoAsync($"{logPrefix}==>|Req_DcbCheck|{conversationId}|Check Service Connectivity");

        //                DcbSystemResponse loginOp = await _dcbServices.QryUserBasicBalOp("218947776156");

        //                if (loginOp.IsSuccessStatusCode)
        //                {
        //                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Connected");
        //                    return true;
        //                }

        //                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Not Connected");
        //                return false;


        //            }
        //            catch (Exception ex)
        //            {
        //                await ExceptionLogs(ex);
        //                return false;
        //            }


        //        }

        /*Chech if Gecol System Reachable or Not :

*/

        public async Task<bool> CheckDcbExist()
        {
            string conversationId = DateTime.Now.ToString($"yyyyMMddHHmmssfff");

            try
            {
                await _loggerG.LogInfoAsync($"{logPrefix}==>|Req_DcbCheck|{conversationId}|Check Service Connectivity");

                Result<SuccessResponseQryUserBasicBal, FailureResponse> subProServiceResp = await _dcbServices.QryUserBasicBalOpX("218947776156");


                if (subProServiceResp.IsSuccess)
                {
                    await _loggerG.LogInfoAsync(($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Connected"));
                    return true;
                }
                else
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Not Connected");
                    return false;
                }              


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

        private async Task<bool> CheckMeterExist(string MeterNumber)
        {
            try
            {

                string meterNumber = subProService.MeterNumber;

                await _loggerG.LogInfoAsync($"{logPrefix}==>|Req_GecolMeter|{conversationId}|Check The Meter|{meterNumber}");

                // Query in DB
                // Uncomment and implement DB query logic here
                // if (condition)
                // {
                // }
                // else if (otherCondition)
                // {
                // }

                var gecolSystem = await _gecolServices.ConfirmCustomerOpx(MeterNumber);

                if (gecolSystem.IsSuccess)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}<==|Rsp_GecolMeter|{conversationId}|The Meter Connected|{meterNumber}");
                    return true;
                }

                await _loggerG.LogInfoAsync($"{logPrefix}<==|Rsp_GecolMeter|{conversationId}|The Meter Number Not Exist or has Issue|{meterNumber}");
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

        private async Task<Result<SuccessResponseDirectDebit, FailureResponse>> ProcessChargeByDCB(SubProService subProService)
        {

            try
            {

                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_BillingSys|{conversationId}|{msisdn}|{amount}");

                Result<SuccessResponseDirectDebit, FailureResponse> subProServiceResp = await _dcbServices.DirectDebitUnitOp(conversationId, msisdn, amount);



                if (subProServiceResp.IsSuccess)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_BillingSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.Success.IsSuccessStatusCode}|{subProServiceResp.Success.Response}");
                }
                else
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_BillingSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.Failure.IsSuccessStatusCode}|{subProServiceResp.Failure.Failure}");
                }






                if (subProServiceResp.IsSuccess)
                {
                    //here ConncetionString to saveing in DB in success case :

                    return Result<SuccessResponseDirectDebit, FailureResponse>.SuccessResult(subProServiceResp.Success);
                }

                //*here ConncetionString to saveing in DB in Failed case :


                return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(subProServiceResp.Failure);

            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);

                FailureResponse subProServiceResp = new FailureResponse()
                {
                    Failure = ex.Message,
                    StatusCode = "error_DCB_PCBD",
                    IsSuccessStatusCode = false
                };

                return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(subProServiceResp);

            }
        }





        /* Rollback balance with Billing :
        * 
        * 1. if charging Success reply with true.
        *
        * 2. if charging Failed reply with false.
        *
         */

        private async Task<TokenOrError> ProcessRollBackDCB(SubProService subProService)
        {

            try
            {

                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_RollBackDCB|{conversationId}|{msisdn}|{amount}");

                DcbSystemResponse subProServiceResp = await _dcbServices.DebitRollbackOp(conversationId, subProService.TransactionID, msisdn, amount);

                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_RollBackDCB|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccessStatusCode}|{subProServiceResp.Response}");

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

        private async Task<Result<SuccessResponseCreditVend, FailureResponse>> ProcessTokenFromGecol(SubProService subProService)
        {

            try
            {
                string msisdn = subProService.MSISDN;
                int amount = subProService.Amount;
                string uniqeNumber = subProService.UniqueNumber;

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolVnSys|{conversationId}|{subProService.MSISDN}|{subProService.Amount}");

                var subProServiceResp = await 
                    _gecolServices.CreditVendOpx(
                        subProService.MeterNumber,
                        subProService.UniqueNumber,
                        subProService.Amount);

                if (subProServiceResp.IsSuccess)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolVnSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccess}|{uniqeNumber}|token|{subProServiceResp.Success.Response.CreditVendTx.STS1Token}");
                }
                else 
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolVnSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccess}|{subProServiceResp.Failure.StatusCode}|{uniqeNumber}|error|{subProServiceResp.Failure.Failure}");
                }


                if (subProServiceResp.IsSuccess)
                {
                    /*here ConncetionString to saveing in DB in success case : 
                    */

                    return Result <SuccessResponseCreditVend, FailureResponse>.SuccessResult(subProServiceResp.Success);
                }
                else
                {
                    /*here ConncetionString to saveing in DB in Failed case :
                     * */

                    return Result<SuccessResponseCreditVend, FailureResponse>.FailureResult(subProServiceResp.Failure);

                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);

                FailureResponse subProServiceResp = new FailureResponse()
                {
                    Failure = ex.Message,
                    StatusCode = "error_GCL_PTFG",
                    IsSuccessStatusCode = false                
                };

                return Result<SuccessResponseCreditVend, FailureResponse>.FailureResult(subProServiceResp);

            }

        }





        /* Send SMS API to SMPP Client  :
        */

        private async Task SendGecolMessage(string? sender, string receiver, string message)
        {
            try
            {
                HttpClient client = new HttpClient();
                string msisdn = subProService.MSISDN;

                var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.31.17:8086/api/Messages");

                SmsMessage jsonObject = new()
                {
                    Receiver = receiver,
                    Message = message
                };

                var content = new StringContent(JsonConvert.SerializeObject(jsonObject), null /*System.Text.Encoding.UTF8*/, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_SMSCSystem|Submet|To|{msisdn}");

                response.EnsureSuccessStatusCode();
                var messageResponse = await response.Content.ReadAsStringAsync();

                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_SMSCSystem|Response|{messageResponse}");
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);
            }
        }





        /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
       */

        private SubProService CreateSubProService(MultiRequest multiRequest, string sessionId)
        {
            string[] Para = multiRequest.USSDRequestString.Split('#');

            if (Para.Length < 2)
            {
                throw new ArgumentException("The USSD request string does not contain enough parameters.");
            }


            //
            //check if the amount value if int or not
            //
            int amount = (int.TryParse(Para[1], out amount)) ? amount : 0;
            
            
            // Create Request Object
            //
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

        private async Task ExceptionLogs(Exception ex)
        {
            await _loggerG.LogDebugAsync(
                  $"excp" +
                  $"\n{subProService.ConversationID}|{ex.Message}" +
                  $"\n{subProService.ConversationID}|{ex.InnerException}" +
                  $"\n{subProService.ConversationID}|{ex.StackTrace.ToString()}"
                  );
        }





        /* Main Class (Service Start here)
         */

        public async Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang)
        {
            /* Service start here

             the logic here use two condtions ,
            
             - Generate Sesstion ID : for Subscriber Request
            
             - Check Meter DBs : Check Meter in database if exist  return with true
            
             - Check Meter API : if not in DB check by API if exist add to DB and return with true
            
             - if not exist reply with false
            */



            /* Generate Sesstion ID :

            */

            string sessionId = DateTime.Now.ToString($"yyyyMMddHHmmssfff");

            if (DateTime.TryParseExact(multiRequest.TransactionTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Format the DateTime object into the desired format
                sessionId = parsedDate.ToString("yyyyMMddHHmmss") + DateTime.Now.ToString("fff");
            }




            /* Logger for start Sesstion :
             */

            await _loggerG.LogInfoAsync($"UssdSystem|==>|Req_{logPrefix}|Start|Session_Id |{sessionId}");





            /* Use this Model for Collect USSD, DCB & GECOL Parameters in one Object:
            */

            subProService = CreateSubProService(multiRequest, sessionId);


            /* Check BlackList :
             * 
             */

            if (await BlackListMsisdn(multiRequest.MSISDN))
            {
                msgContentResult = await _menus.BlockedResponseAsync(Lang);

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
                        var GecolOrderResult = await ProcessTokenFromGecol(subProService);

                        if (GecolOrderResult.IsSuccess)
                        {
                            var DcbOrderResult = await ProcessChargeByDCB(subProService);


                            if (DcbOrderResult.IsSuccess)
                            {

                                msgContentResult = await _menus.SuccessResponseAsync(GecolOrderResult.Success, Lang);

                                if (!string.IsNullOrEmpty(msgContentResult.MessageCont))
                                {
                                    _sendMessage.SendGecolMessage(subProService.MSISDN, msgContentResult.MessageCont, subProService.ConversationID);
                                }
                                else
                                {
                                    msgContentResult = await _menus.UnderMaintenance_Billing(DcbOrderResult.Failure.StatusCode, Lang);
                                }
                            }
                            else
                            {
                                //
                                // record request in DB issues
                                //

                                await _loggerG.LogIssuedTokenAsync($"IssuedToken|-x-|Rsp_{logPrefix}|Session_Id |{sessionId}|token|{GecolOrderResult.Success.Response.CreditVendTx.STS1Token}");


                                msgContentResult = await _menus.UnderMaintenance_Billing(DcbOrderResult.Failure.StatusCode, Lang);

                            }
                            

                        }
                        else
                        {
                            msgContentResult = await _menus.UnderMaintenance_Gecol(GecolOrderResult.Failure.StatusCode, Lang);
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
                    msgContentResult = await _menus.UnderMaintenance_Gecol("VD.01010018", Lang);
                }
            }
            else
            {
                msgContentResult = await _menus.UnderMaintenance_Gecol(default, Lang);
            }

            // Generate USSD Response

            await _loggerG.LogInfoAsync($"UssdSystem|<==|Rsp_{logPrefix}|Close|Session_Id |{sessionId}");

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