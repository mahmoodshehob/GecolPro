using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DefaultValues = GecolPro.SmppClient.Models.DefaultValue;

namespace GecolPro.SmppClient.Models
{
    public class RabbitMsgQ
    {
        //public RabbitMsgQ()
        //{
        //    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //    string jsonFilePath = Path.Combine(baseDirectory, "defaultValue.json");
        //    string json = File.ReadAllText(jsonFilePath);

        //    var jsonObject = JObject.Parse(json);
        //    var rabbitMsgQJson = jsonObject["RabbitMsgQ"]?.ToString();
        //    var defaultValues = JsonConvert.DeserializeObject<DefaultValue.RabbitMsgQ>(rabbitMsgQJson);


        //    HostName = defaultValues!.HostName;
        //    Port = defaultValues.Port!;
        //    UserName = defaultValues!.UserName;
        //    Password = defaultValues.Password;
        //    VirtualHost = defaultValues!.VirtualHost;
        //}


        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }

        public string QueueName { get; set; }
    }
}