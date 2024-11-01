namespace RapsAssessment_ShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }

    public static class ProductData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "ASUS Vivobook 16",
                    Description = "Intel Core i3-1215U 12th Gen, Thin and Light Laptop, 16-inch (40.64 cm) FHD+.",
                    Price = 32000,
                    ImageUrl = "https://m.media-amazon.com/images/I/41-f8GLgejL._SX300_SY300_QL70_ML2_.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Acer Aspire Lite",
                    Description = "AMD Ryzen 5-5625U Premium Thin and Light Laptop (16 GB RAM/512 GB SSD/Windows 11 Home).",
                    Price = 50000,
                    ImageUrl = "https://m.media-amazon.com/images/I/41Bmui2T3TL._SX300_SY300_QL70_FMwebp_.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "HP Laptop 15s",
                    Description = "AMD Ryzen 7 5700U, 15.6-inch(39.6 cm) FHD, Anti-Glare Laptop (16 GB/512 GB/Backlit KB/Win 11.",
                    Price = 48400,
                    ImageUrl = "https://m.media-amazon.com/images/I/41JSeHmZ-nL._SX300_SY300_QL70_ML2_.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Dell 15 Laptop",
                    Description = "3th Gen Intel Core i5-1335U Processor/ 8GB/ 512GB SSD//Windows 11.",
                    Price = 48900,
                    ImageUrl = "https://m.media-amazon.com/images/I/61GxTXDZhPL._SX679_.jpg"
                }
            };
        }
    }
}
