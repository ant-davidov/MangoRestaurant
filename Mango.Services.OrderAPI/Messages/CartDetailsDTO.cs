﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Messeges
{
    public class CartDetailsDTO
    {
        public int CartDetailsId { get; set; }
        public int CartHeadrtId { get; set; }

        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int Count { get; set; }
    }
}
