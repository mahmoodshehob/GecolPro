using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using GecolPro.SmppClient.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using GecolPro.SmppClient.Services.IServices;

namespace GecolPro.SmppClient.Services
{
    public class MessageWorker : BackgroundService
    {

        private static DlrMessage dlrMessage = new DlrMessage();

        private readonly ConnectionFactory _factory;

        private readonly HttpClient _client;

        private readonly string _queueName;

        private Loggers _loggers = new Loggers();

        private readonly KannelModel _kannelModel;

        private readonly RabbitMsgQ rabbitMQ;

        public MessageWorker(IConfiguration config )
        {

            _kannelModel = new KannelModel();
            config.GetSection("KannelPara").Bind(_kannelModel);

            rabbitMQ = new RabbitMsgQ();
            config.GetSection("RabbitMsgQ").Bind(rabbitMQ);
            _queueName = rabbitMQ.QueueName;



            _factory = new ConnectionFactory()
            {


                HostName = rabbitMQ.HostName,
                UserName = rabbitMQ.UserName,
                Password = rabbitMQ.Password,
                VirtualHost = rabbitMQ.VirtualHost,
                Port = rabbitMQ.Port //Protocol.DefaultPort// default is 5672 for non-SSL
            };
               
            _client = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var message = JsonSerializer.Deserialize<SmsToKannel>(json);


                        dlrMessage.msgId = message.msgID;

                        string dlr_uri = $"dlr-mask={_kannelModel.DlrMask}&dlr-url={_kannelModel.DlrEndPoint}/Messages/DLR/{dlrMessage.Phone}/{dlrMessage.msgId}/{dlrMessage.Status}/%t/";

                        Uri url = new Uri($"http://{_kannelModel.HostName}:{_kannelModel.Port}/cgi-bin/sendsms?username=kannel&password=kannel&from={message.Sender}&to=%2B{message.Receiver}&charset=UTF-8&coding=2&text={message.Message}&{dlr_uri}");

                        if (!String.IsNullOrEmpty(message.Profile) || !String.IsNullOrWhiteSpace(message.Profile))
                        {  
                            url = new Uri($"http://{_kannelModel.HostName}:{_kannelModel.Port}/cgi-bin/sendsms?username=kannel&password=kannel&smsc={message.Profile}&from={message.Sender}&to=%2B{message.Receiver}&charset=UTF-8&coding=2&text={message.Message}&{dlr_uri}");
                        }

                        try
                        {
                            var response = await _client.GetAsync(url);

                            if (response.IsSuccessStatusCode)
                            {
                                await _loggers.LogInfoAsync($"{ea.DeliveryTag.ToString()}");

                                // Log or handle the successful response
                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            }
                            else
                            {
                                // Log the failure but do not requeue automatically
                                await _loggers.LogInfoAsync($"Failed to send SMS: {response.StatusCode}");
                                channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                            }
                        }
                        catch (Exception ex)
                        {
                            await _loggers.LogInfoAsync($"Exception occurred: {ex.Message}");
                            // Requeue the message to try again
                            channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                        }

                        // Delete_MQ(_queueName);

                    };

                    channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggers.LogInfoAsync($"Exception occurred: {ex.Message}");
            }
        }

        protected async Task CheckAsync(CancellationToken stoppingToken)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var data = channel.BasicGet(_queueName, autoAck: false);
                if (data != null)
                {
                    var message = Encoding.UTF8.GetString(data.Body.ToArray());
                    await _loggers.LogInfoAsync($"Received message: {message}");

                    // Acknowledge the message if needed
                    // channel.BasicAck(data.DeliveryTag, multiple: false);
                }
            }


        }

        private async Task Error()
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    // Messaging code here
                }
            }
            catch (BrokerUnreachableException ex)
            {
                await _loggers.LogInfoAsync("Could not reach the broker: " + ex.Message);
            }
            catch (Exception ex)
            {
                await _loggers.LogInfoAsync("An error occurred: " + ex.Message);
            }


        }

        private void Delete_MQ(string QueueName)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDelete(QueueName);
                channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            }
        }

    }
}