using Mango.Services.OrderAPI.Messeges;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Mango.Services.OrderAPI.Messaging
{
    public class ServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;
        public ServiceBusConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        private async Task OnCheckoutMessageReceives(object args)
        {
            const string queueName = "PaymentQueue";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, e) =>
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
                            Price =order.Product.Price,
                            Count = order.Count
                        };
                        orderHeader.CartTotalItems += order.Count;
                        orderHeader.OrderDetails.Add(orderDetails);

                    }

                    await _orderRepository.AddOrder(orderHeader);
                };
                
               
            };
        }
    }
}
