
using Newtonsoft.Json;

namespace ClassLibrary.DCBSystem_Update.Models
{
    public class AuthHeader
    {
        public AuthHeader()
        {
            var json = File.ReadAllText("defaultValues.json");
            var defaultValues = JsonConvert.DeserializeObject<DefaultValues>(json);

            Username = defaultValues!.Username;
            Password = defaultValues.Password;
            Url = new Uri(defaultValues.Url);
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public Uri Url { get; set; }
    }
}