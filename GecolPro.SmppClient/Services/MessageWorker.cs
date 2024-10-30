﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using GecolPro.SmppClient.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GecolPro.SmppClient.Services
{
    public class MessageWorker : BackgroundService
    {


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






                        var url = $"http://{_kannelModel.HostName}:{_kannelModel.Port}/cgi-bin/sendsms?username=kannel&password=kannel&from={message.Sender}&to=%2B{message.Receiver}&charset=UTF-8&coding=2&text={message.Message}&dlr-mask=31&dlr-url=http://{_kannelModel.HostName}:8089/dlr?status=%d&msgid=%k";
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
                                Console.WriteLine($"Failed to send SMS: {response.StatusCode}");
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
                    Console.WriteLine($"Received message: {message}");

                    // Acknowledge the message if needed
                    // channel.BasicAck(data.DeliveryTag, multiple: false);
                }
            }


        }

        private void Error()
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
                Console.WriteLine("Could not reach the broker: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
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