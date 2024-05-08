using GecolPro.Main.Data;
using GecolPro.Main.Models;
using static System.Net.Mime.MediaTypeNames;


namespace GecolPro.Main.Process
{
    public class SmsActions

    {
        //private readonly ApplicationDbContext _context;

        //private SmsActions(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly string DefaultSender =  "NSM";

        private string KannelUrl()
        {
            string ipAddress = "172.16.206.2";

            int Port = 13013;
            
            string username = "kannel";
            
            string password = "kannel";
            
            string Url = "http://"+ipAddress+":"+Port+"/cgi-bin/sendsms?username="+username+"&password="+ password + "&from="; 

            return Url;
        }

        public async Task SubmitSms(string Sender, string Reciver, string Message)
        {
            await Task.Run(async () =>
            {

                if (string.IsNullOrEmpty(Sender))
                {
                    Sender = DefaultSender; // Set your desired default sender value here
                }


                Reciver = Reciver.Replace("+", "");
                Reciver = Reciver.Substring(0, 12);

                try
                {
                    var client = new HttpClient();
                    string URL = KannelUrl() + Sender + "&to=" + Reciver + "&charset=UTF-8&coding=1&text=" + Message;
                    var request = new HttpRequestMessage(HttpMethod.Get, URL);
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //
                    //
                    //
                }
                catch (Exception ex)
                {
                  //  await EsmeChecker.Logger.Logger.Log("EsmeChecker.Process.SmsActions.PostSms" + "|" + "Exception" + "|" + ex.Message);
                }


            });
        }

        //public async Task SubmitCma(string Sender, string Reciver, string Message)
        //{
        //    await Task.Run(async () =>
        //    {

        //        //if (string.IsNullOrEmpty(Sender))
        //        //{
        //        //    Sender = "Libyana"; // Set your desired default sender value here
        //        //}


        //        Sender = "Libyana";
        //        Reciver = Reciver.Replace("+", "");
        //        Reciver = Reciver.Substring(0, 12);



        //            try
        //            {
        //                var client = new HttpClient();
        //                //string URL = "http://172.16.206.5:13013/cgi-bin/sendsms?username=kannel&password=kannel&from=" + Sender + "&to=" + Reciver + "&charset=UTF-8&coding=1&text=" + Message;
        //                string URL = "http://172.16.206.5:13013/cgi-bin/sendsms?username=kannel&password=kannel&from=" + Sender + "&to=" + "218947776156" + "&charset=UTF-8&coding=1&text=" + Message+Reciver;
        //                var request = new HttpRequestMessage(HttpMethod.Get, URL);
        //                var response = await client.SendAsync(request);
        //                response.EnsureSuccessStatusCode();
        //                //
        //                //
        //                //
        //            }
        //            catch (Exception ex)
        //            {
        //                await EsmeChecker.Logger.Logger.Log("EsmeChecker.Process.SmsActions.PostSms" + "|" + "Exception" + "|" + ex.Message);

        //            }

        //    });
        //}

        public async void PostSms(List<string> Recivers, string Message)
        {
            string Sender = "2188997771";
            try
            {
                foreach (string Reciver in Recivers)
                {
                    if(float.TryParse(Reciver, out float number) && Reciver.Length==12)
                    {
                        var client = new HttpClient();

                        string URL = "http://172.29.54.125:13013/cgi-bin/sendsms?username=kannel&password=kannel&from=" + Sender + "&to=" + Reciver + "&text=" + Message;
                        var request = new HttpRequestMessage(HttpMethod.Get, URL);
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
