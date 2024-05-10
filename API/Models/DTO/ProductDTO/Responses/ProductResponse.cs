﻿namespace API.Models.DTO.ProductDTO.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public BrandDto Brand { get; set; }
        public List<ProductItemDto> ProductItems { get; set; } = new();
        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<ProductSpecificationDto> ProductSpecifications { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}