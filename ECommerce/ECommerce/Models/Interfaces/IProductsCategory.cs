namespace ECommerce.Models.Interfaces
{
    public interface IProductsCategory
    {

        Task<List<ProductsCategory>> GetAllProductsForCategory(int categoryId);
          Task<ProductsCategory> GetProductCategoryById(int id);
    }
}
