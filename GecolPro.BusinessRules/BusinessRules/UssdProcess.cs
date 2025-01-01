using GecolPro.BusinessRules.Interfaces;
using GecolPro.DataAccess.Interfaces;

// Class Library

using GecolPro.DCBSystem;
using GecolPro.GecolSystem;
using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;
using static GecolPro.Models.Models.MultiRequestUSSD;

//Models

using GecolPro.Models.Models;
using GecolPro.Services.IServices;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;



namespace GecolPro.BusinessRules.BusinessRules
{
    public class UssdProcess : IUssdProcess
    {

        private const string contentType = "text/xml";
        private readonly AuthHeader _authHeader;
        private readonly Random random = new Random();
        private readonly MappingPKgs _mappingPkgs;


        private IMenusX? _menus;
        private ILoggers _loggerG ;
        private IDcbServices? _dcbServices;
        private IGecolServices? _gecolServices;
        private IUssdConverter _ussdConverter;
        private IResponses _responses;


        private ISendMessage? _sendMessage;
        private readonly IDbUnitOfWork? _dbunitOfWork;


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
            IConfiguration _config,
            IUssdConverter ussdConverter,
            IResponses responses,
            IDbUnitOfWork? dbunitOfWork)
        {
            _dcbServices = dcbServices;
            _gecolServices = gecolServices;
            _loggerG = loggerG;
            _menus = menus;
            _sendMessage = sendMessage;
            _dbunitOfWork = dbunitOfWork;
            _ussdConverter = ussdConverter;
            _responses = responses;
            _authHeader = new AuthHeader()
            {
                Username = _config.GetValue<string>("AuthHeaderOfDCB:username"),
                Password = _config.GetValue<string>("AuthHeaderOfDCB:password"),
                Url      = _config.GetValue<string>("AuthHeaderOfDCB:url")
            };

            _mappingPkgs = new MappingPKgs();
            _config.GetSection("MappingPKgs").Bind(_mappingPkgs.Mappings);


        }

        private class RespActions
        {
            public const string request = "request";
            public const string end = "end";
        }


        //private class Respresponse
        //{
        //    public const string True = "True";
        //    public const string False = "False";  
        //}





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

        public async Task<bool> CheckServiceExist(string? convID =null)
        {
            try
            {
                Result<SuccessResponseLogin, FailureResponse> loginOp;

                if (!string.IsNullOrWhiteSpace(conversationId))
                {
                    // this log for normal request

                    convID = conversationId;

                    await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolCheck|{convID}|Check Service Connectivity");

                    loginOp = await _gecolServices.LoginReqOp();

                    if (loginOp.IsSuccess)
                    {
                        await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{convID}|Service Connected");
                        return true;
                    }

                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{convID}|Service Not Connected");
                    return false;
                }
                else
                {

                    //this to check status by API

                    await _loggerG.LogConnectionsStatusAsync($"{logPrefix}==>|Req_GecolCheck|{convID}|Check Service Connectivity");

                    loginOp = await _gecolServices.LoginReqOp();

                    if (loginOp.IsSuccess)
                    {
                        await _loggerG.LogConnectionsStatusAsync($"{logPrefix}|<==|Rsp_GecolCheck|{convID}|Service Connected");
                        return true;
                    }

                    await _loggerG.LogConnectionsStatusAsync($"{logPrefix}|<==|Rsp_GecolCheck|{convID}|Service Not Connected");
                    return false;
                }


            }
            catch (Exception ex)
            {
                convID=ex.GetType().Name;
                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolCheck|{convID}|Service Not Connected");

                await ExceptionLogs(ex);
                return false;
            }


        }





        /*Chech if Gecol System Reachable or Not :

*/

        public async Task<bool> CheckDcbExist(string? convID = null)
        {
            string conversationId = DateTime.Now.ToString($"yyyyMMddHHmmssfff");

            try
            {
                await _loggerG.LogConnectionsStatusAsync($"{logPrefix}==>|Req_DcbCheck|{conversationId}|Check Service Connectivity");

                Result<SuccessResponseQryUserBasicBal, FailureResponse> subProServiceResp = await _dcbServices.QryUserBasicBalOp("218947776156");


                if (subProServiceResp.IsSuccess)
                {
                    await _loggerG.LogConnectionsStatusAsync(($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Connected"));
                    return true;
                }
                else
                {
                    await _loggerG.LogConnectionsStatusAsync($"{logPrefix}|<==|Rsp_DcbCheck|{conversationId}|Service Not Connected");
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

        private async Task<bool> CheckMeterExist(string meterNumber)
        {
            try
            {
                // Log the start of the DB check
                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_DB|{conversationId}|Check The Meter|{meterNumber}");

                var meterCheckResult = await _dbunitOfWork.Meter.IsExist(meterNumber);
                
                if (meterCheckResult.Status)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_DB|{conversationId}|The Meter Exist|{meterNumber}");
                    
                    return true;
                }

                // Log and check meter in Gecol
                await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_DB|{conversationId}|The Meter not Exist|{meterNumber}");

                return await CheckMeterInGecol(meterNumber);
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exception: Attempt to check in Gecol
                await _loggerG.LogErrorAsync($"{logPrefix}|xxx|SqlException|{conversationId}|Error|{sqlEx.Message}");
                
                return await CheckMeterInGecol(meterNumber);
            }
            catch (Exception ex)
            {
                await _loggerG.LogErrorAsync($"{logPrefix}|xxx|GeneralException|{conversationId}|Error|{ex.Message}");
                await ExceptionLogs(ex);
                return false;
            }
        }





        /* check in Gecol by API.
*/

        private async Task<bool> CheckMeterInGecol(string meterNumber)
        {
            try
            {
                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolMeter|{conversationId}|Check The Meter|{meterNumber}");

                var gecolResponse = await _gecolServices.ConfirmCustomerOp(meterNumber);

                if (gecolResponse.IsSuccess)
                {
                    await AddMeterToDB(meterNumber, gecolResponse.Success);

                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolMeter|{conversationId}|The Meter Connected|{meterNumber}|MinAmount:{gecolResponse.Success.Response.MinVendAmt}");
                    
                    return true;
                }
                else
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolMeter|{conversationId}|The Meter Issued|{meterNumber}|ErrorCode|{gecolResponse.Failure.StatusCode}|Error Desc:|{gecolResponse.Failure.Failure}");
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                await _loggerG.LogErrorAsync($"{logPrefix}|xxx|CheckMeterInGecol|{conversationId}|Error|{ex.Message}");
                
                await ExceptionLogs(ex);
                
                return false;
            }
        }


        /* check in Gecol by API.
*/

        public async Task<(bool,string)> CheckMeterInGecolWithDet(string meterNumber)
        {
            try
            {
                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolMeter|{conversationId}|Check The Meter|{meterNumber}");

                var gecolResponse = await _gecolServices.ConfirmCustomerOp(meterNumber);

                if (gecolResponse.IsSuccess)
                {
                    await AddMeterToDB(meterNumber, gecolResponse.Success);

                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolMeter|{conversationId}|The Meter Connected|{meterNumber}|MinAmount:{gecolResponse.Success.Response.MinVendAmt}");

                    return (true,"Meter Owner : " +gecolResponse.Success.Response.CustVendDetail.Name);
                }
                else
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolMeter|{conversationId}|The Meter Issued|{meterNumber}|ErrorCode|{gecolResponse.Failure.StatusCode}|Error Desc:|{gecolResponse.Failure.Failure}");

                    return (false, gecolResponse.Failure.StatusCode);
                }
            }
            catch (Exception ex)
            {
                await _loggerG.LogErrorAsync($"{logPrefix}|xxx|CheckMeterInGecol|{conversationId}|Error|{ex.Message}");

                await ExceptionLogs(ex);

                return (false, ex.Message);
            }
        }
        /* Add Meter to DataBase.
         */

        private async Task AddMeterToDB(string MeterNumber , SuccessResponseConfirmCustomer CheckGecolMeter)
        {
      
                if ((await _dbunitOfWork.Meter.CreateNew(MeterNumber, CheckGecolMeter.Response.AT, CheckGecolMeter.Response.TT, CheckGecolMeter.Response.CustVendDetail)).Status)
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|+++|Add_Meter_ToDB|{conversationId}|The Meter Saved|{MeterNumber}");
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
                int amount = subProService.AmountDCB;

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_BillingSys|{conversationId}|{msisdn}|{amount}");

                Result<SuccessResponseDirectDebit, FailureResponse> subProServiceResp = await _dcbServices.DirectDebitUnitOp(conversationId, msisdn, amount);



                if (subProServiceResp.IsSuccess)
                {
                    subProService.TransactionID = subProServiceResp.Success.Response.TransactionID;
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_BillingSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.Success.IsSuccessStatusCode}|TransactionID|{subProServiceResp.Success.Response.TransactionID}|Amount|{subProServiceResp.Success.Response.Amount}");
                }
                else
                {
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_BillingSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.Failure.IsSuccessStatusCode}|ErrorCode:{subProServiceResp.Failure.StatusCode}|ErrorDesc:{subProServiceResp.Failure.Failure}");
                }



                if (subProServiceResp.IsSuccess)
                {
                    //here ConncetionString to saveing in DB in success case :
                    if ((await _dbunitOfWork.Request.SaveDcblRequest(
                        conversationId: subProService.ConversationID,
                        MSISDN: subProService.MSISDN,
                        meterNumber: subProService.MeterNumber,
                        amount: subProService.AmountDCB.ToString(),
                        status: subProServiceResp.IsSuccess,
                        transactionId: subProService.TransactionID))
                        .Status)
                    {
                        await _loggerG.LogRequstDbAsync("DCB" + "|" +
                        subProService.ConversationID + "|" +
                        subProService.MSISDN + "|" +
                        subProService.AmountDCB.ToString() + "|" +
                        subProServiceResp.IsSuccess + "|" +
                        subProService.TransactionID);
                    }




                    return Result<SuccessResponseDirectDebit, FailureResponse>.SuccessResult(subProServiceResp.Success);
                }

                //*here ConncetionString to saveing in DB in Failed case :

                if (!(await _dbunitOfWork.IssueToken.CreateNew(
                    conversationId: subProService.ConversationID,
                    msisdn: subProService.MSISDN,
                    dateTimeReq: DateTime.Now.ToString("yyyy/MM/dd HH:MM:ss"),
                    uniqueNumber: subProService.UniqueNumber,
                    meterNumber: subProService.MeterNumber,
                    amount: subProService.AmountDCB)).Status)
                {
                    await _loggerG.LogIssuedTokenAsync(
                        "conversationId : " + subProService.ConversationID
                        + "|" + "msisdn : " + subProService.MSISDN
                        + "|" + "dateTimeReq : " + DateTime.Now.ToString("yyyy/MM/dd HH:MM:ss")
                        + "|" + "uniqueNumber : " + subProService.UniqueNumber
                        + "|" + "meterNumber : " + subProService.MeterNumber
                        + "|" + "amount : " + subProService.AmountDCB);
                }




                return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(subProServiceResp.Failure);

            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex);

                try
                {
                    await _dbunitOfWork.IssueToken.CreateNew(
                        conversationId: subProService.ConversationID,
                        msisdn: subProService.MSISDN,
                        dateTimeReq: DateTime.Now.ToString("yyyy/MM/dd HH:MM:ss"),
                        uniqueNumber: subProService.UniqueNumber,
                        meterNumber: subProService.MeterNumber,
                        amount: subProService.AmountDCB);
                }
                catch
                {
                    await _loggerG.LogIssuedTokenAsync(
                        "conversationId : " + subProService.ConversationID
                        + "|" + "msisdn : " + subProService.MSISDN
                        + "|" + "dateTimeReq : " + DateTime.Now.ToString("yyyy/MM/dd HH:MM:ss")
                        + "|" + "uniqueNumber : " + subProService.UniqueNumber
                        + "|" + "meterNumber : " + subProService.MeterNumber
                        + "|" + "amount : " + subProService.AmountDCB);
                }

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
                int amount = subProService.AmountDCB;

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
                int amount = subProService.AmountGecol;
                string uniqeNumber = subProService.UniqueNumber;

                await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_GecolVnSys|{conversationId}|{subProService.MSISDN}|Amount|{amount}|MeterNumber|{subProService.MeterNumber}|UniqeNumber|{uniqeNumber}");

                var subProServiceResp = await 
                    _gecolServices.CreditVendOp(
                        subProService.MeterNumber,
                        subProService.UniqueNumber,
                        amount);

                  


                if (subProServiceResp.IsSuccess)
                {
                    var succResp = subProServiceResp.Success.Response;


                    string[] Tokens;
                    if (!string.IsNullOrEmpty(succResp.CreditVendTx.Desc_KcToken))
                    {
                        Tokens = [
                            succResp.CreditVendTx.Set1stMeterKey ,
                            succResp.CreditVendTx.Set2ndMeterKey ,
                            succResp.CreditVendTx.STS1Token];
                        
                        await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolVnSys|{conversationId}|{msisdn}|Amount|{amount}|MeterNumber|{subProService.MeterNumber}|UniqeNumber|{uniqeNumber}|{subProServiceResp.IsSuccess}|1ST|{Tokens[0]}|2ND|{Tokens[1]}|token|{Tokens[2]}");
                    }
                    else
                    {
                        Tokens = [succResp.CreditVendTx.STS1Token];

                        await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_GecolVnSys|{conversationId}|{msisdn}|Amount|{amount}|MeterNumber|{subProService.MeterNumber}|UniqeNumber|{uniqeNumber}|{subProServiceResp.IsSuccess}|token|{Tokens[0]}");
                    }

                    subProService.TransactionID = subProServiceResp.Success.Response.CreditVendReceipt;


                    /*here ConncetionString to saveing in DB in success case : 
                    */

                    var SaveGecolRequest = await _dbunitOfWork.Request.SaveGecolRequest(
                        conversationId  : subProService.ConversationID,
                        MSISDN          : subProService.MSISDN,
                        meterNumber     : subProService.MeterNumber,
                        amount          : amount.ToString(),
                        status          : subProServiceResp.IsSuccess,
                        transactionId   : subProService.TransactionID,
                        token           : Tokens,
                        uniqueNumber    : subProService.UniqueNumber,
                        totalTax        : succResp.CreditVendTx.Amout);
                     
                    string logToken = "";
                    foreach (var tkn in Tokens){ logToken += tkn + ";"; }
                    
                    await _loggerG.LogInfoAsync($"{logPrefix}|==>|Req_Save_Trans|{conversationId}|{msisdn}|Amount|{amount}|MeterNumber|{subProService.MeterNumber}|UniqeNumber|{uniqeNumber}|{subProServiceResp.IsSuccess}|token|{Tokens[0]}");
                    
                    

                    if (SaveGecolRequest.Status)
                    { 
                        await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_Save_Trans|{conversationId}|{SaveGecolRequest.Status}");
                    }
                    else
                    {
                        await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsp_Save_Trans|{conversationId}|{SaveGecolRequest.Status}");

                        await _loggerG.LogRequstDbAsync("Gecol"
                            + "|" + subProService.ConversationID
                            + "|" + subProService.MSISDN
                            + "|" + amount.ToString()
                            + "|" + subProServiceResp.IsSuccess
                            + "|" + logToken
                            + "|" + subProService.UniqueNumber
                            + "|" + succResp.CreditVendTx.Amout);

                    }


                    return Result <SuccessResponseCreditVend, FailureResponse>.SuccessResult(subProServiceResp.Success);
                }
                else
                {


                    
                    await _loggerG.LogInfoAsync($"{logPrefix}|<==|Rsq_GecolVnSys|{conversationId}|{msisdn}|{amount}|{subProServiceResp.IsSuccess}|uniqeNumber : {uniqeNumber}|ErrorCode:{subProServiceResp.Failure.StatusCode}|ErrorDesc:{subProServiceResp.Failure.Failure}");

                    /*here ConncetionString to saveing in DB in Failed case :
                     * */
                    await _dbunitOfWork.IssueToken.CreateNew(
                        subProService.ConversationID,
                        subProService.MSISDN,
                        DateTime.Now.ToString(),
                        subProService.UniqueNumber,
                        subProService.MeterNumber,
                        amount);

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
            int amountDcb = (int.TryParse(Para[1], out amountDcb)) ? amountDcb : 0;
            int amountGcl = _mappingPkgs.Mappings.TryGetValue(amountDcb, out var value) ? value : amountDcb-3;

            // Create Request Object
            //
            SubProService subProService = new()
            {
                ConversationID = sessionId,

                MSISDN = multiRequest.MSISDN,

                DateTimeReq = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"),

                MeterNumber = Para[0],

                AmountDCB = amountDcb,

                AmountGecol = amountGcl


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




        // save the invoice in database.

        private async Task SaveMessageToDB()
        {

        }

        private async Task SaveMessageToDB(object obj)
        {

        }





        /* Main Class (Service Start here)
         */

        private async Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang)
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
                    Action = RespActions.end,
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
                                    // save the invoice in database.

                                    await SaveMessageToDB();

                                    // send invoice by sms to subscriber .

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
                Action = RespActions.end,
            };

            return multiResponse;
        }






        public async Task<ContentResult> GetResponse(string xmlContent, string lang)
        {
            ContentResult response = new ContentResult();

            await _loggerG.LogUssdTransAsync($"{xmlContent}");

            MultiRequestUSSD.MultiRequest multiRequest = await _ussdConverter.ConverterFaster(xmlContent);

            MultiResponseUSSD multiResponse = await ServiceProcessing(multiRequest, lang);


            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                string respContetn = _responses.Resp(multiResponse);

                await _loggerG.LogUssdTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 200
                };
                return response;

            }
            else
            {
                string respContetn = _responses.Fault_Response(multiResponse);

                await _loggerG.LogDcbTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 400
                };
                return response;
            }
        }


   


        /* Main Class (Service Start here)
         */

        private async Task<MultiResponseUSSD> QueryTokenHistoryProcessing(MultiRequest multiRequest, string Lang)
        {
            MultiResponseUSSD _multiResponseUSSD;

            /* Service start here

             the logic here use two condtions ,
            
             - Use MSISDN to query on token in last 30 days.

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


            try
            {
                await _loggerG.LogInfoAsync($"DatabaseQry|==>|Req_{logPrefix}|Msisdn|{multiRequest.USSDRequestString}");

                var _records = await _dbunitOfWork.Request.QueryTokenHistoryAll(multiRequest.USSDRequestString);



                if (_records != null)
                {

                    await _loggerG.LogInfoAsync($"DatabaseQry|<==|Req_{logPrefix}|CountOfTokens|{_records.Count()}");

                    var recordHistory = await _menus.HistoryRecordsAsync(_records, Lang);

                    await _sendMessage.SendGecolMessage( multiRequest.MSISDN, recordHistory.MessageCont , conversationId);


                    return _multiResponseUSSD = new MultiResponseUSSD
                    {
                        TransactionId = multiRequest.TransactionId,
                        TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                        MSISDN = multiRequest.MSISDN,
                        USSDServiceCode = "0",
                        USSDResponseString = recordHistory.UssdCont,
                        Action = RespActions.end,
                        ResponseCode = 0
                    };
                }

                return _multiResponseUSSD = new MultiResponseUSSD
                {
                    TransactionId = multiRequest.TransactionId,
                    TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                    MSISDN = multiRequest.MSISDN,
                    USSDServiceCode = "0",
                    USSDResponseString = "ليس لديك طلبات في اخر 30 يوم.",
                    Action = RespActions.end,
                    ResponseCode = 0
                };

            }
            catch (Exception ex)
            {
                return _multiResponseUSSD = new MultiResponseUSSD
                {
                    TransactionId = multiRequest.TransactionId,
                    TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                    MSISDN = multiRequest.MSISDN,
                    USSDServiceCode = "0",
                    USSDResponseString = ex.Message,
                    Action = RespActions.end,
                    ResponseCode = 400
                };
            }


        }




        public async Task<ContentResult> GetQueryTokensResponse(string xmlContent, string lang)
        {
            ContentResult response = new ContentResult();

            MultiRequestUSSD.MultiRequest multiRequest = await _ussdConverter.ConverterFaster(xmlContent);


            MultiResponseUSSD multiResponse = await QueryTokenHistoryProcessing(multiRequest, lang);

            await _loggerG.LogUssdTransAsync($"{xmlContent}");

            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                string respContetn = _responses.Resp(multiResponse);

                await _loggerG.LogUssdTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 200
                };
                return response;

            }
            else
            {
                string respContetn = _responses.Fault_Response(multiResponse);

                await _loggerG.LogDcbTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 400
                };
                return response;
            }
        }

        public async Task<(bool,string)> GetQueryTokensResponseSupport(string Msisdn, string OrderdNumber, string lang)
        {

            ContentResult response = new ContentResult();

            MultiRequestUSSD.MultiRequest multiRequest = new()
            {
                TransactionId = "support" + new Random().Next(),
                TransactionTime = DateTime.Now.ToString("yyyyMMddTHH:mm:ss"),
                MSISDN = Msisdn,
                USSDRequestString = OrderdNumber,
                USSDServiceCode = "apiInt",
                Response    =   null
            };


            MultiResponseUSSD multiResponse = await QueryTokenHistoryProcessing(multiRequest, lang);

            //await _loggerG.LogUssdTransAsync($"{xmlContent}");

            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                return (true, multiResponse.USSDResponseString);
            }
            else
            {
                return (false, multiResponse.USSDResponseString);
            }
        }
    }
}