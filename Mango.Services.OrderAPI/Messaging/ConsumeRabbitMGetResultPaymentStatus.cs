using Mango.MessageBus;
using Mango.Services.OrderAPI.Repository;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Mango.Services.OrderAPI.Messages;
using Newtonsoft.Json;

namespace Mango.Services.OrderAPI.Messaging
{
    public class ConsumeRabbitMGetResultPaymentStatus : BackgroundService
    {
        const string queueName = "OrderPaymentResult";
        private IConnection _connection;
        private IModel _channel;
        private readonly OrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;
        public ConsumeRabbitMGetResultPaymentStatus(OrderRepository orderRepository, IMessageBus messageBus)
        {
            InitRabbitMQ();
            _orderRepository = orderRepository;
            _messageBus = messageBus;
        }
        private void InitRabbitMQ()
        {

            var factory = new ConnectionFactory { HostName = "localhost" };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var paymentResultMessage = JsonConvert.DeserializeObject<UpdatepaymentResultMessage>(message);
                await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId,paymentResultMessage.Status);

                _channel.BasicAck(e.DeliveryTag, false);


                // Console.WriteLine("ReaD!!!!!!!!!!!!!!");
            };
            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;

        }



    }
    
}
