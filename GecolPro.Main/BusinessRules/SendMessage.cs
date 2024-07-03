using ClassLibrary.Models.Models;
using ClassLibrary.Services;
using Newtonsoft.Json;

namespace GecolPro.Main.BusinessRules
{
    public class SendMessage
    {
        private static Loggers LoggerG = new Loggers();

        /* Send SMS API to SMPP Client  :
 */

        public static async void SendGecolMessage(string? sender, string receiver, string message, string ConversationID)
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

                    await LoggerG.LogInfoAsync($"LynaGclsys|==>|Req_SMSCSystem|Submet|To|{receiver}");

                    response.EnsureSuccessStatusCode();
                    var messageResponse = await response.Content.ReadAsStringAsync();

                    await LoggerG.LogInfoAsync($"LynaGclsys|<==|Rsp_SMSCSystem|Respon|{messageResponse}");
                }
            }
            catch (Exception ex)
            {
                await ExceptionLogs(ex, ConversationID);
            }
        }

        private static async Task ExceptionLogs(Exception ex , string ConversationID)
        {
            await LoggerG.LogDebugAsync(
                  $"excp" +
                  $"\n{ConversationID}|{ex.Message}" +
                  $"\n{ConversationID}|{ex.InnerException}" +
                  $"\n{ConversationID}|{ex.StackTrace.ToString()}"
                  );
        }


    }
}
