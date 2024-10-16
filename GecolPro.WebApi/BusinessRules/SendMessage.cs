using GecolPro.Models.Models;
using GecolPro.Services.IServices;
using GecolPro.WebApi.Interfaces;
using Newtonsoft.Json;

namespace GecolPro.WebApi.BusinessRules
{
    public class SendMessage : ISendMessage

    {
        private ILoggers _loggerG;

        /* Send SMS API to SMPP Client  :*/


        public SendMessage(ILoggers loggerG)
        {
            _loggerG = loggerG;

        }

        public  async Task SendGecolMessage(string? sender, string receiver, string message, string ConversationID)
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
