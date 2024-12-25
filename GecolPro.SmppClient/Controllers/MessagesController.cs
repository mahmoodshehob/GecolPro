using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using GecolPro.SmppClient.Models;
using GecolPro.SmppClient.Services;
using GecolPro.SmppClient.Services.IServices;
using System.Text.RegularExpressions;
using System.Numerics;

//namespace GecolPro.SmppClient.Controllers
//{


//    //[Route("api/[controller]")]
//    [ApiController]
//    public class MessagesController : ControllerBase
//    {
//        //private readonly ConnectionFactory _factory;


//        private IGuidService _msgID;
//        private MessageProfile messageProfile;

//        private readonly KannelModel _kannelModel;


//        private ConnectionFactory _factory;
//        private readonly string _queueName;
//        private RabbitMsgQ rabbitMQ;
//        private ILoggers _loggers ;


//        public MessagesController(IConfiguration config ,IGuidService msgID, ILoggers loggers)
//        {
//            // Guid
//            _msgID = msgID;


//            // Loggers

//            _loggers = loggers;

//            // Kannel

//            _kannelModel = new KannelModel();
//            config.GetSection("KannelPara").Bind(_kannelModel);

//            // rabbitmq

//            rabbitMQ = new RabbitMsgQ();
//            config.GetSection("RabbitMsgQ").Bind(rabbitMQ);

//            _queueName = rabbitMQ.QueueName;


//            _factory = new ConnectionFactory()
//            {

//                HostName = rabbitMQ.HostName,
//                UserName = rabbitMQ.UserName,
//                Password = rabbitMQ.Password,
//                VirtualHost = rabbitMQ.VirtualHost,
//                // Port = AmqpTcpEndpoint.UseDefaultPort
//                Port = rabbitMQ.Port //Protocol.DefaultPort// default is 5672 for non-SSL

//            };
//        }





//        [HttpPost]
//        [Route("api/[controller]/Post")]
//        public async Task<IActionResult> Post([FromBody] SmsToKannel message)
//        {
//            if (message.Profile.ToLower() == "string")
//            {
//                message.Profile=null;
//            }

//            if (!IsValidPositiveLong(message.Receiver))
//            {
//                return BadRequest("the phone Number not rigth.");
//            }




//            messageProfile = new MessageProfile()
//            {
//                MsgID = _msgID.GetGuid(),
//                Sender = message.Sender,
//                Receiver = message.Receiver,
//                Message= message.Message,
//                Profile = message.Profile
//            };





//            var json = JsonSerializer.Serialize(messageProfile);


//            await _loggers.LogInfoAsync($"Submit|msgid:{messageProfile.MsgID}|{JsonSerializer.Serialize(message).ToString()}");

//            try
//            {
//                using (var connection = _factory.CreateConnection())
//                {
//                    Console.WriteLine("Connection to RabbitMQ created.");
//                    using (var channel = connection.CreateModel())
//                    {
//                        // Console.WriteLine("Channel created. Declaring queue in MessagesController.");
//                        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
//                        var body = Encoding.UTF8.GetBytes(json);
//                        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
//                        await _loggers.LogInfoAsync($"sms to Q");
//                        // Console.WriteLine("Message published to queue.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);

//            }
//            return Ok();
//        }




//        [HttpGet("[controller]/DLR/{phone}/{msgid}/{status}/{deliveryDate}")]
//        public async Task<IActionResult> DLR(string phone, string msgid, string status, string deliveryDate)
//        {
//            try
//            {
//                phone = phone.Replace("+", "");

//                await _loggers.LogInfoAsync($"Delivr|msgid:{msgid},phone:{phone},status:{status}, deliveryDate:{deliveryDate}");

//                // Validate parameters
//                if (string.IsNullOrEmpty(msgid))
//                {
//                    return BadRequest("Message ID (msgid) is required.");
//                }

//                // Process the request (dummy response for demonstration)
//                var response = new
//                {
//                    MessageId = msgid,
//                    Status = status,
//                    Timestamp = DateTime.UtcNow
//                };

//                return Ok(response); // Return the response as JSON
//            }
//            catch (Exception ex)
//            {
//                await _loggers.LogInfoAsync($"exp|{ex.Message}");

//                return BadRequest(ex.Message);

//            }
//        }




//        [HttpGet()]
//        [Route("[controller]/Status")]
//        public async Task<IActionResult> KannelStatus()
//        {
//            try
//            {
//                var client = new HttpClient();
//                var request = new HttpRequestMessage(HttpMethod.Get, $"http://{_kannelModel.HostName}:13000/status?password=kannel");
//                var response = await client.SendAsync(request);
//                response.EnsureSuccessStatusCode();


//                if (response.IsSuccessStatusCode)
//                {
//                    string rrr = await response.Content.ReadAsStringAsync();

//                    return Ok(JsonSerializer.Serialize(parcing(rrr))); // Return the response as JSON

//                }




//                return BadRequest();

//            }
//            catch (Exception ex)
//            {
//                await _loggers.LogInfoAsync($"exp|{ex.Message}");

//                return BadRequest(ex.Message);

//            }
//        }








//        private bool IsValidPositiveLong(string input)
//        {
//            // Check if the input length matches the required length
//            if (input.Length != 12)
//            {
//                return false;
//            }

//            // Check if the input is a valid positive long
//            if (long.TryParse(input, out long result) && result > 0)
//            {
//                return true;
//            }

//            return false;
//        }


//        private KannelStatus parcing(string bodystatus)
//        {
//            string inputText = bodystatus;

//            // Corrected Box Connections Regex
//            string boxConnectionsPattern = @"Box connections:\s+smsbox:\((.*?)\), IP ([0-9.]+) \((\d+) queued\), \(on-line (.*?)\)";
//            MatchCollection boxMatches = Regex.Matches(inputText, boxConnectionsPattern);

//            var boxConnections = new List<object>();

//            foreach (Match match in boxMatches)
//            {
//                boxConnections.Add(new
//                {
//                    Type = "smsbox",
//                    Details = new
//                    {
//                        Name = match.Groups[1].Value,
//                        IP = match.Groups[2].Value,
//                        Queued = int.Parse(match.Groups[3].Value),
//                        OnlineTime = match.Groups[4].Value
//                    }
//                });
//            }

//            // SMSC Connections Regex (unchanged)
//            string smscConnectionsPattern = @"\s*(.*?)\s*SMPP:(.*?):(.*?)/(.*?):(.*?):(.*?) \(online (\d+)s, rcvd: sms (\d+) \((.*?)\) / dlr (\d+) \((.*?)\), sent: sms (\d+) \((.*?)\) / dlr (\d+) \((.*?)\), failed (\d+), queued (\d+) msgs\)";
//            MatchCollection smscMatches = Regex.Matches(inputText, smscConnectionsPattern);

//            var smscConnections = new List<object>();

//            foreach (Match smscMatch in smscMatches)
//            {




//                smscConnections.Add(new
//                {
//                    Name = smscMatch.Groups[1].Value.Trim(),
//                    Protocol = "SMPP",
//                    Address = $"{smscMatch.Groups[2].Value}:{smscMatch.Groups[3].Value}/{smscMatch.Groups[4].Value}",
//                    ConnectionType = $"{smscMatch.Groups[3].Value}/{smscMatch.Groups[4].Value}",
//                    //Account = $"{smscMatch.Groups[5].Value}:{smscMatch.Groups[6].Value}",
//                    Account = $"{smscMatch.Groups[5].Value}",

//                    Status = new
//                    {
//                        OnlineSeconds = int.Parse(smscMatch.Groups[7].Value),
//                        Received = new
//                        {
//                            Sms = new
//                            {
//                                Count = int.Parse(smscMatch.Groups[8].Value),
//                                Rate = smscMatch.Groups[9].Value.Split(',')
//                            },
//                            Dlr = new
//                            {
//                                Count = int.Parse(smscMatch.Groups[10].Value),
//                                Rate = smscMatch.Groups[11].Value.Split(',')
//                            }
//                        },
//                        Sent = new
//                        {
//                            Sms = new
//                            {
//                                Count = int.Parse(smscMatch.Groups[12].Value),
//                                Rate = smscMatch.Groups[13].Value.Split(',')
//                            },
//                            Dlr = new
//                            {
//                                Count = int.Parse(smscMatch.Groups[14].Value),
//                                Rate = smscMatch.Groups[15].Value.Split(',')
//                            }
//                        },
//                        Failed = int.Parse(smscMatch.Groups[16].Value),
//                        Queued = int.Parse(smscMatch.Groups[17].Value)
//                    }
//                });







//            }

//            // Combine results into a single object
//            var result = new
//            {
//                BoxConnections = boxConnections,
//                SMSCConnections = smscConnections
//            };

//            // Serialize to JSON
//            string jsonResult = JsonSerializer.Serialize(result);
//            var kannelStatus = JsonSerializer.Deserialize<KannelStatus>(jsonResult);

//            if (kannelStatus.SMSCConnections.Count() > 0)
//            {
//                for (int i = 0; i <= (kannelStatus.SMSCConnections.Count() - 1); i++)
//                {
//                    switch ($"{kannelStatus.SMSCConnections[i].ConnectionType}")
//                    {
//                        case "5016/0":
//                            kannelStatus.SMSCConnections[i].ConnectionType = "Transmitter";
//                            break;
//                        case "0/5016":
//                            kannelStatus.SMSCConnections[i].ConnectionType = "Receiver";
//                            break;
//                        case "5016/5016":
//                            kannelStatus.SMSCConnections[i].ConnectionType = "Transceiver";
//                            break;
//                        default:
//                            kannelStatus.SMSCConnections[i].ConnectionType = "Transceiver";
//                            break;
//                    }
//                }
//            }

//            return kannelStatus;
//        }
//    }
//}



namespace GecolPro.SmppClient.Controllers
{


    //[Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        //private readonly ConnectionFactory _factory;


        private IServiceLogic _serviceLogic;

        public MessagesController(IServiceLogic serviceLogic)
        {
            _serviceLogic =  serviceLogic;
        }





        [HttpPost]
        [Route("api/[controller]/Post")]
        public async Task<IActionResult> Post([FromBody] SmsToKannel message)
        {

            var _result = await _serviceLogic.Post(message);
            
            if (_result.Item1)
            { 
                return Ok(_result.Item2); 
            }
            else
            {
                return BadRequest(_result.Item2);
            }

        }


        [HttpGet("[controller]/DLR/{phone}/{msgid}/{status}/{deliveryDate}")]
        public async Task<IActionResult> DLR(string phone, string msgid, string status, string deliveryDate)
        {
            var _result = await _serviceLogic.DLR(phone , msgid , status, deliveryDate);

            if (_result.Item1)
            {
                return Ok(_result.Item2);
            }
            else
            {
                return BadRequest(_result.Item2);
            }
        }




        [HttpGet()]
        [Route("[controller]/Status")]
        public async Task<IActionResult> KannelStatus()
        {
            var _result = await _serviceLogic.KannelStatus();

            if (_result.Item1)
            {
                return Ok(_result.Item2);
            }
            else
            {
                return BadRequest(_result.Item2);
            }
        }

    }
}