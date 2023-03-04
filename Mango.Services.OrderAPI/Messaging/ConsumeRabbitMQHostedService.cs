using Mango.Services.OrderAPI.Messeges;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mango.Services.OrderAPI.Messaging
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        const string queueName = "PaymentQueue";
        private IConnection _connection;
        private IModel _channel;
        private readonly OrderRepository _orderRepository;
        public ConsumeRabbitMQHostedService(OrderRepository orderRepository)
        {
            InitRabbitMQ();
            _orderRepository = orderRepository;
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
                var checkoutHeaderDTO = JsonConvert.DeserializeObject<CheckoutHeaderDTO>(message);
                OrderHeader orderHeader = new OrderHeader()
                {
                    UserId = checkoutHeaderDTO.UserId,
                    FisrtName = checkoutHeaderDTO.FisrtName,
                    LastName = checkoutHeaderDTO.LastName,
                    OrderDetails = new List<OrderDetails>(),
                    CardNumber = checkoutHeaderDTO.CardNumber,
                    CouponCode = checkoutHeaderDTO.CouponCode,
                    CVV = checkoutHeaderDTO.CVV,
                    DiscountTotal = checkoutHeaderDTO.DiscountTotal,
                    Email = checkoutHeaderDTO.Email,
                    ExpiryMonthYear = checkoutHeaderDTO.ExpiryMonthYear,
                    OrderTime = DateTime.UtcNow,
                    OrderTotl = checkoutHeaderDTO.OrderTotl,
                    PaymentStatus = false,
                    Phone = checkoutHeaderDTO.Phone,
                    PickupDateTime = checkoutHeaderDTO.PickupDateTime
                };
                foreach (var order in checkoutHeaderDTO.CartDetails)
                {
                    OrderDetails orderDetails = new()
                    {
                        ProductId = order.ProductId,
                        ProductName = order.Product.Name,
                        Price = order.Product.Price,
                        Count = order.Count
                    };
                    orderHeader.CartTotalItems += order.Count;
                    orderHeader.OrderDetails.Add(orderDetails);

                }

                await _orderRepository.AddOrder(orderHeader);
                _channel.BasicAck(e.DeliveryTag, false);
               // Console.WriteLine("ReaD!!!!!!!!!!!!!!");
            };
            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;

        }
        
        

    }
}
