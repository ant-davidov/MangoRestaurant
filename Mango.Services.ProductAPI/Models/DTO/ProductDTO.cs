﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Range(1, 1000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}