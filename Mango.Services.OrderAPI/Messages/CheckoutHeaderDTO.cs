

namespace Mango.Services.OrderAPI.Messeges
{
    public class CheckoutHeaderDTO 
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotl { get; set; }
        public double DiscountTotal { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string Phone { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public int CartTotalItems { get; set; }
        public IEnumerable<CartDetailsDTO> CartDetails { get; set; }
    }
}
