using Microsoft.AspNetCore.SignalR;
using PositionsService.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace PositionsService.Services
{
    public class RabbitMqService
    {
        private readonly IHubContext<PositionHub> _hubContext;
        private readonly string _queueName = "positionsQueue";

        public RabbitMqService(IHubContext<PositionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Publish message to RabbitMQ
        public void PublishMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [x] Sent: {message}");
            }
        }

        // Consume messages on the queue and notifies through SignalR
        public void ConsumeMessages()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received: {message}");

                // Notify to the clients using SignalR
                await _hubContext.Clients.All.SendAsync("PositionUpdated", message);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}