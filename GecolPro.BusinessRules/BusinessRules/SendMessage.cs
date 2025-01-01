using GecolPro.Models.SMPP;
using GecolPro.Models.Models;
using GecolPro.Services.IServices;
using GecolPro.BusinessRules.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Azure;
using GecolPro.BusinessRules.UssdService;
using System.Runtime.InteropServices.JavaScript;
using System.Text;


namespace GecolPro.BusinessRules.BusinessRules
{
    public class SendMessage : ISendMessage

    {
        private ILoggers _loggerG;
        private readonly SmppInfo _smppInfo;
        private HttpClient client;

        /* Send SMS API to SMPP Client  :*/

        public SendMessage(IConfiguration _config,ILoggers loggerG)
        {
            _loggerG = loggerG;

            client = new HttpClient();

            _smppInfo = new SmppInfo()
            {
                Sender  =    _config.GetValue<string>("SmmpInfo:Sender"),
                Url     =    _config.GetValue<string>("SmmpInfo:url"),
                Profile =   _config.GetValue<string>("SmmpInfo:Profile")

            };
            if (_smppInfo.Profile == "" || string.IsNullOrEmpty(_smppInfo.Profile))
                _smppInfo.Profile = null;
        }

        public async Task SendGecolMessage(string receiver, string message, string ConversationID)
        {
            try
            {

                if (!string.IsNullOrEmpty(message))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, _smppInfo.Url);

                    SmsMessage MessageObject = new SmsMessage()
                    {
                        msgID = string.Empty,
                        Sender = _smppInfo.Sender,
                        Receiver = receiver,
                        Message = message,
                        Profile = string.Empty
                    };

                    string jsonObject;

                    jsonObject = JsonConvert.SerializeObject(MessageObject);

                    var content = new StringContent(jsonObject, null, "application/json");

                    request.Content = content;

                    HttpResponseMessage response = await client.SendAsync(request);

                    await _loggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Endpoint|{_smppInfo.Url}|Submet|To|{receiver}");

                    if (response.IsSuccessStatusCode) 
                    {
                        response.EnsureSuccessStatusCode();
                        var messageResponse = await response.Content.ReadAsStringAsync();                        
                        await _loggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex, ConversationID);
              
            }
        }

        public async Task SendGecolMessageTest(string receiver, string message, string ConversationID)
        {
            try
            {

                if (!string.IsNullOrEmpty(message))
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.31.118:8086/api/Messages/Post");
                    request.Headers.Add("accept", "*/*");
                    var content = new StringContent("{\n  \"sender\": \"Gecol\",\n  \"receiver\": \"218947776156\",\n  \"message\": \"string\",\n  \"profile\": \"\"\n}", null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);

                    await _loggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Endpoint|{_smppInfo.Url}|Submet|To|{receiver}");

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        var messageResponse = await response.Content.ReadAsStringAsync();
                        await _loggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex, ConversationID);

            }
        }


        public async Task<(bool,object)> SendGecolMessageWR(string receiver, string message, string ConversationID)
        {
            try
            {

                if (!string.IsNullOrEmpty(message))
                {
                    var request = new HttpRequestMessage(method: HttpMethod.Post,requestUri:_smppInfo.Url);
                    
                    request.Headers.Add("accept", "*/*");

                    SmsMessage jsonObject = new SmsMessage()
                    {
                        Sender = _smppInfo.Sender,
                        Receiver = receiver,
                        Message = message,
                        Profile = _smppInfo.Profile
                    };


                    var content = new StringContent(
                        JsonConvert.SerializeObject(jsonObject),
                        null,
                        "application/json");

                    //var content = new StringContent(
                    //JsonConvert.SerializeObject(new SmsMessage()
                    //{
                    //    Sender = _smppInfo.Sender,
                    //    Receiver = receiver,
                    //    Message = message,
                    //    Profile = _smppInfo.Profile
                    //}),
                    //null,
                    //"application/json");


                    request.Content = content;

                    var response = await client.SendAsync(request);

                    await _loggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Endpoint|{_smppInfo.Url}|Submet|To|{receiver}");
                    
                    var messageResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        await _loggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                    }
                    return (response.IsSuccessStatusCode, messageResponse);
                }
                return (false, "Message empity");

            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex, ConversationID);
                return (false, ex.Message);

            }
        }


        private async Task ExceptionLogs(Exception ex, string ConversationID)
        {
            await _loggerG.LogDebugAsync(
                  $"excp" +
                  $"\n{ConversationID}|{ex.Message}" +
$"\n{ConversationID}|{ex.InnerException}" +
$"\n{ConversationID}|{ex.StackTrace.ToString()}"
                  );
        }
    }  
}
