using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public void PublishMessage(BaseMessage message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            message.Id = Guid.NewGuid().ToString();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                      exclusive:false,
                                      autoDelete: false,
                                      arguments: null);
               
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            };
            
                

        }
    }
}
