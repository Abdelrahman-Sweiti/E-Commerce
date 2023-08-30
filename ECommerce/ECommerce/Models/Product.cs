namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ImageUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }


        public List<ProductsCart>? productsCarts { get; set; }
        public List<ProductsCategory>? productsCategories { get; set; }
        public List<ProductsOrder>? productsOrders { get; set; }
    }
}
