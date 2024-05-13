using Newtonsoft.Json;

namespace ClassLibrary.GecolSystem_Update.Models
{
    public class AuthCred
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }


        public AuthCred()
        {
            var json = File.ReadAllText("defaultValues.json");
            var defaultValues = JsonConvert.DeserializeObject<DefaultValues>(json);
            Username = defaultValues.Username!;
            Password = defaultValues.Password!;
            Url = defaultValues.Url!;
        }
    }
}