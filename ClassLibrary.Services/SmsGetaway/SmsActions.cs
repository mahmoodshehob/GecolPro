using System;
using System.Threading.Tasks;
using ClassLibrary.Services.Models;

namespace ClassLibrary.Services.SmsGetaway
{
    public class SmsActions

    {

        private readonly string SectionName = "SMPP_Section";
        private IniFiles.IniFiles iniFiles = new IniFiles.IniFiles();
        private Dictionary<string,string> dictionary = new Dictionary<string, string>(); 
        private readonly string DefaultSender = "2188997772";



        private string KannelUrl()
        {
            AuthSMPP authSMPP = new AuthSMPP();
            try 
            {
                dictionary = iniFiles.Read(SectionName);

                if (!string.IsNullOrEmpty(dictionary["IpAddress"]))
                { authSMPP.IpAddress = dictionary["IpAddress"]; }
                
                if (!string.IsNullOrEmpty(dictionary["Port"]))
                { authSMPP.Port = dictionary["Port"]; }

                if (!string.IsNullOrEmpty(dictionary["Username"]))
                { 
                    authSMPP.Username = dictionary["Username"];
                }

                if (!string.IsNullOrEmpty(dictionary["Password"]))
                { authSMPP.Password = dictionary["Password"]; }
            }
            catch { }

            //string Url = "http://" + authSMPP.IpAddress + ":" + authSMPP.Port + "/cgi-bin/sendsms?username=" + authSMPP.Username + "&password=" + authSMPP.Password + "&from=";

            //return Url;
            return "http://" + authSMPP.IpAddress + ":" + authSMPP.Port + "/cgi-bin/sendsms?username=" + authSMPP.Username + "&password=" + authSMPP.Password + "&from=";
        }

        //public string Responce { get; set; }
        //public int StatusCode { get; set; }
        //public Boolean state { get; set; }

        public async Task<(string Responce, string StatusCode, Boolean state)> SubmitSms(string? sender, string receiver, string message)
        {
            MessageData messageData = new MessageData()
            {
                
                Receiver = receiver,    
                Message = message
            };

            try
            {
                if (messageData == null)
                {
                   
                    return ("Invalid message data",(401).ToString(),false);
                
                }

                messageData.Receiver = messageData.Receiver.Replace("+", "");
                messageData.Receiver = messageData.Receiver.Substring(0, 12);
                
                
                
                var client = new HttpClient();
                
                string URL = KannelUrl() + messageData.Sender + "&to=" + messageData.Receiver + "&charset=UTF-8&coding=1&text=" + messageData.Message;
                
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                
                HttpResponseMessage response = await client.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {

                    return ("Message sent successfully", (200).ToString(), true);

                }
                else
                {
                    return ("Failed to reach server", (400).ToString(), false);

                }
            }
            catch (Exception ex)
            {
                //  await EsmeChecker.Logger.Logger.Log("EsmeChecker.Process.SmsActions.PostSms" + "|" + "Exception" + "|" + ex.Message);

                return (ex.Message, (400).ToString(), false);
                
            }
        }
    }
}
