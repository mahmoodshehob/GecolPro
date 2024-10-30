using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using GecolPro.SmppClient.Models;
using GecolPro.SmppClient.Services;
using GecolPro.SmppClient.Services.IServices;

namespace GecolPro.SmppClient.Controllers
{


    //[Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        //private readonly ConnectionFactory _factory;
        private ConnectionFactory _factory;
        private readonly string _queueName;
        private RabbitMsgQ rabbitMQ;
        private Loggers _loggers = new Loggers();


        public MessagesController(IConfiguration config)
        {
            // rabbitmq

            rabbitMQ = new RabbitMsgQ();
            config.GetSection("RabbitMsgQ").Bind(rabbitMQ);

            _queueName = rabbitMQ.QueueName;

     
            _factory = new ConnectionFactory()
            {

                HostName = rabbitMQ.HostName,
                UserName = rabbitMQ.UserName,
                Password = rabbitMQ.Password,
                VirtualHost = rabbitMQ.VirtualHost,
                // Port = AmqpTcpEndpoint.UseDefaultPort
                Port = rabbitMQ.Port //Protocol.DefaultPort// default is 5672 for non-SSL

            };
        }





        [HttpPost]
        [Route("api/[controller]/Post")]
        public async Task<IActionResult> Post([FromBody] SmsToKannel message)
        {
            
            var json = JsonSerializer.Serialize(message);


            await _loggers.LogInfoAsync($"Submit|{json.ToString()}");

            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    Console.WriteLine("Connection to RabbitMQ created.");
                    using (var channel = connection.CreateModel())
                    {
                        Console.WriteLine("Channel created. Declaring queue in MessagesController.");
                        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        var body = Encoding.UTF8.GetBytes(json);
                        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                        await _loggers.LogInfoAsync($"sms to Q");
                        Console.WriteLine("Message published to queue.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return Ok();
        }




        [HttpPost]
        [Route("api/[controller]/DLR")]
        public async Task<IActionResult> DLR([FromQuery] string id, [FromQuery] string status, [FromQuery] string timestamp, [FromQuery] string destination)
        {
            try
            {
                // Log the received delivery report
                await _loggers.LogInfoAsync($"DLR|ID:{id}, Status:{status}, Timestamp:{timestamp}, Destination:{destination}");

                return Ok(); // Respond with a success message
            }
            catch (Exception ex)
            {
                await _loggers.LogErrorAsync($"Error in MessagesController: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}