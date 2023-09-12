namespace ECommerce.Models.Interfaces
{
    public interface IProduct
    {
        Task<Product> Create(Product product  );
        Task<ProductsCategory> AddCategoryToProduct(int categoryId, int productId);
        Task<Product> GetFile(IFormFile file, Product product);
        Task<Product> GetProductById(int productId);

    }
}
