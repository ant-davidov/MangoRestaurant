using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Model.DTO
{
    public class CartDetailsDTO
    {
        public int CartDetailsId { get; set; }
        public int CartHeadrtId { get; set; }
        public virtual CartHeaderDTO CartHeader { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int Count { get; set; }
    }
}
