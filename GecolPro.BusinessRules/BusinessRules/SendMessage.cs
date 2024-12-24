using GecolPro.Models.SMPP;
using GecolPro.Models.Models;
using GecolPro.Services.IServices;
using GecolPro.BusinessRules.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace GecolPro.BusinessRules.BusinessRules
{
    public class SendMessage : ISendMessage

    {
        private ILoggers _loggerG;
        private readonly SmppInfo _smppInfo;

        /* Send SMS API to SMPP Client  :*/


        public SendMessage(IConfiguration _config,ILoggers loggerG)
        {
            _loggerG = loggerG;
            _smppInfo = new SmppInfo()
            {
                Sender  =    _config.GetValue<string>("SmmpInfo:Sender"),
                Url     =    _config.GetValue<string>("SmmpInfo:url"),
                Profile =   _config.GetValue<string>("SmmpInfo:Profile")

            };
            if (_smppInfo.Profile == "" || string.IsNullOrEmpty(_smppInfo.Profile))
                _smppInfo.Profile = null;
        }

        public  async Task SendGecolMessage(string receiver, string message, string ConversationID)
        {
            try
            {

                if (!string.IsNullOrEmpty(message))
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, _smppInfo.Url);

                    SmsMessage jsonObject = new SmsMessage()
                    {
                        Sender = _smppInfo.Sender,
                        Receiver = receiver,
                        Message = message,
                        Profile = _smppInfo.Profile
                    };


                    var content = new StringContent(JsonConvert.SerializeObject(jsonObject), null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);

                    await _loggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Submet|To|{receiver}");

                    response.EnsureSuccessStatusCode();
                    var messageResponse = await response.Content.ReadAsStringAsync();

                    await _loggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex, ConversationID);
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
