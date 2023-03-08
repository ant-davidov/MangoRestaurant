using Mango.MessageBus;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;

namespace Mango.Services.Email.Messaging
{
    public class GetResultPaymentStatusForEmail : BackgroundService
    {
        const string queueName = "OrderPaymentResultForSendEmail";
        private IConnection _connection;
        private IModel _channel;
        private readonly EmailRepository _emailRepository;
        private readonly IMessageBus _messageBus;
        public GetResultPaymentStatusForEmail(EmailRepository emailRepository, IMessageBus messageBus)
        {
            InitRabbitMQ();
            _emailRepository = emailRepository;
            _messageBus = messageBus;
        }
        private void InitRabbitMQ()
        {

            var factory = new ConnectionFactory { HostName = "localhost" };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            //_channel.QueueDeclare(queue: queueName,
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

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
                await _emailRepository.SendAndLogEmail(paymentResultMessage);
                _channel.BasicAck(e.DeliveryTag, false);


                // Console.WriteLine("ReaD!!!!!!!!!!!!!!");
            };
            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;

        }



    }
    
}
