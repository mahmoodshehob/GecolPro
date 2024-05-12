namespace ClassLibrary.GecolSystem_Update.Models
{
    public class AuthCred
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }


        public AuthCred(string username = "AG0502", string password = "1234567891012", string url = "http://160.19.103.138:40808/xmlvend/xmlvend.wsdl")
        {
            Username = username;
            Password = password;
            Url = url;
        }
    }
}