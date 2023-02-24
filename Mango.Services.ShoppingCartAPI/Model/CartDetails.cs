using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Model
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; }
        public int CartHeadrtId { get; set;}
        [ForeignKey("CartHeaderId")]
        public virtual CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Count { get; set; }
    }
}
