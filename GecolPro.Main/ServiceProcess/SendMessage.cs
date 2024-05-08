//using ClassLibrary.Services.SmsGetaway;
//using ClassLibrary.Services.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace GecolPro.Main.ServiceProcess
//{
//    public class SendMessage
//    {

//        public async Task<MessageResponse> PostMessage(MessageData messageData)
//        {
//            SmsActions smsActions = new SmsActions();

//            var result = await smsActions.SubmitSms(messageData);

//            //
//            // Log the incoming message data
//            //await _logger.LogInfoAsync($"Received message from {messageData.Sender} to {messageData.Receiver}");
//            //

//            return new MessageResponse()
//            {
//                Responce = result.Responce,
//                StatusCode = result.StatusCode,
//                state = result.state
//            };

//        }


//        public static async Task<MessageResponse> PostMessageX(MessageData messageData)
//        {
//            SmsActions smsActions = new SmsActions();

//            var result = await smsActions.SubmitSms(messageData);

//            //
//            // Log the incoming message data
//            //await _logger.LogInfoAsync($"Received message from {messageData.Sender} to {messageData.Receiver}");
//            //

//            return new MessageResponse()
//            {
//                Responce = result.Responce,
//                StatusCode = result.StatusCode,
//                state = result.state
//            };

//        }
//    }
//}
