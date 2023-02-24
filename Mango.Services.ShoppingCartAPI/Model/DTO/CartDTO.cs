namespace Mango.Services.ShoppingCartAPI.Model.DTO
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
