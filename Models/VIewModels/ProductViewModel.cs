namespace Web_Clone_Ebay.Models.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public string ImageUrls { get; set; }
        public double? AverageRating { get; set; }
    }

    public class ProductDetailResultVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ImageUrlVM> ListImage { get; set; } = new List<ImageUrlVM>();
    }
}
public class ImageUrlVM
{
    public string ImageUrl { get; set; }
}