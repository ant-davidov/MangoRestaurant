using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace Mango.Services.PaymentAPI
{
    public class ConsumeRabbitMQPaymentService : BackgroundService
    {
        const string queueNameForRead = "OrderPaymentProcessTopic";
        private IConnection _connection;
        private IModel _channel;
        private readonly IProcessPayment _processPayment;
 
        private readonly IMessageBus _messageBus;
        public ConsumeRabbitMQPaymentService(IMessageBus messageBus,IProcessPayment processPayment )
        {
            InitRabbitMQ();
            _messageBus = messageBus;  
            _processPayment = processPayment;
        }
        private void InitRabbitMQ()
        {
           
            var factory = new ConnectionFactory { HostName = "localhost" };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueNameForRead,
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

                var paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(message);
                var result = _processPayment.PaymentProcessor(); // need a real payment service
                UpdatepaymentResultMessage updatepaymentResultMessage = new()
                {
                    Status = result,
                    OrderId = paymentRequestMessage.OrderId
                };

                _channel.BasicAck(e.DeliveryTag, false);
                try
                {
                    string queueNamePush = "OrderPaymentResult";
                    _messageBus.PublishMessage(updatepaymentResultMessage, queueNamePush);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                // Console.WriteLine("ReaD!!!!!!!!!!!!!!");
            };
            _channel.BasicConsume(queueNameForRead, false, consumer);

            return Task.CompletedTask;

        }
        
        

    }
}
