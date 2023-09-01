namespace ECommerce.Models.Interfaces
{
    public interface IProduct
    {
        Task<Product> Create(Product product  );
        Task<ProductsCategory> AddCategoryToProduct(int categoryId, int productId);
    }
}
