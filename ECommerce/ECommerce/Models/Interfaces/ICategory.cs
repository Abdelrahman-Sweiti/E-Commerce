namespace ECommerce.Models.Interfaces
{
    public interface ICategory
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<Product> AddProductToCategories(int categoriesId, Product product  );

    }
}
